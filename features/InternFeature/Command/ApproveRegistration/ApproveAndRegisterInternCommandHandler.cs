using LMS___Mini_Version.Domain.Constants;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Command.ApproveRegistration;

public class ApproveAndRegisterInternCommandHandler(
    IUnitOfWork unitOfWork,
    IGeneralRepository<Intern> internRepository,
    IGeneralRepository<Track> trackRepository,
    IGeneralRepository<Enrollment> enrollmentRepository,
    IGeneralRepository<Payment> paymentRepository)
    : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(
        ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
    {
        // payment method must be provided and valid
        if (request.PaymentMethod is null || !Enum.IsDefined(request.PaymentMethod.Value))
            return Fail(400, "PaymentMethod",
                "PaymentMethod is required and must be one of: Cash, CreditCard, BankTransfer, Online");

        var method = request.PaymentMethod.Value;

        // checks + writes in ONE serializable transaction (all or nothing)
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            // intern must exist and be Pending
            var intern = await internRepository.GetByIdAsync(request.InternId);
            if (intern is null)
                return Fail(404, "InternId", "Intern not found");

            if (intern.Status != InternStatus.Pending)
                return Fail(409, "Status",
                    $"Only a Pending intern can be approved (current status: {intern.Status})");

            // the intern's assigned track must exist and be active
            var track = await trackRepository.GetByIdAsync(intern.TrackId);
            if (track is null)
                return Fail(404, "TrackId", "Track not found");

            if (!track.IsActive)
                return Fail(409, "TrackId", "Track is not active");

            // no other Pending/Active enrollment for the same intern + track
            var alreadyEnrolled = await enrollmentRepository.GetAll()
                .AnyAsync(e => e.InternId == intern.Id
                            && e.TrackId == track.Id
                            && (e.Status == EnrollmentStatus.Pending
                                || e.Status == EnrollmentStatus.Active),
                    cancellationToken);
            if (alreadyEnrolled)
                return Fail(409, "Enrollment",
                    "Intern already has a Pending or Active enrollment in this track");

            // a seat must be available (same rule as the availability endpoint)
            var reservedSeats = await enrollmentRepository.GetAll()
                .CountAsync(e => e.TrackId == track.Id
                              && (e.Status == EnrollmentStatus.Pending
                                  || e.Status == EnrollmentStatus.Active),
                    cancellationToken);
            if (reservedSeats >= track.MaxCapacity)
                return Fail(409, "Track", "No available seats in this track");

            // build everything
            var now = DateTime.UtcNow;

            intern.Status = InternStatus.Accepted;

            var enrollment = new Enrollment
            {
                InternId = intern.Id,
                TrackId = track.Id,
                Status = EnrollmentStatus.Active,
                EnrollmentDate = now
            };

            var payment = new Payment
            {
                Enrollment = enrollment,
                Amount = track.Fees,              // the track's CURRENT fees
                Status = PaymentStatus.Completed,
                Method = method,
                PaymentDate = now
            };

            enrollmentRepository.Add(enrollment);
            paymentRepository.Add(payment);

            // intern + enrollment + payment saved together
            await unitOfWork.SaveChangesAsync();

            return RequestResponse<int>.OK(enrollment.Id, 201);
        },
        cancellationToken);
    }

    private static RequestResponse<int> Fail(int code, string key, string message)
        => RequestResponse<int>.Fail(code, new Dictionary<string, string[]> { { key, [message] } });
}