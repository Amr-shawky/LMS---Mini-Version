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
        // payment method must be provided and be a valid enum value
        if (request.PaymentMethod is null || !Enum.IsDefined(request.PaymentMethod.Value))
            return Fail(400, "PaymentMethod",
                "PaymentMethod is required and must be one of: Cash, CreditCard, BankTransfer, Online");

        // intern exists
        var intern = await internRepository.GetByIdAsync(request.InternId);
        if (intern is null)
            return Fail(404, "InternId", "Intern not found");

        // only Pending
        if (intern.Status != "Pending")
            return Fail(400, "Status", $"Only a Pending intern can be approved (current status: {intern.Status})");

        // track exists and active
        var track = await trackRepository.GetByIdAsync(intern.TrackId);
        if (track is null)
            return Fail(404, "TrackId", "Track not found");

        if (!track.IsActive)
            return Fail(400, "TrackId", "Track is not active");

        // no duplicate Pending/Active enrollment (same intern + track)
        var alreadyEnrolled = await enrollmentRepository.GetAll()
            .AnyAsync(e => e.InternId == intern.Id
                        && e.TrackId == track.Id
                        && EnrollmentQueryExtensions.ReservingStatuses.Contains(e.Status),
                cancellationToken);
        if (alreadyEnrolled)
            return Fail(409, "Enrollment", "Intern already has a Pending or Active enrollment in this track");

        // available seat (same rule as the availability endpoint)
        var reservedSeats = await enrollmentRepository.GetAll()
            .CountReservedSeatsAsync(track.Id, cancellationToken);

        if (reservedSeats >= track.MaxCapacity)
            return Fail(409, "Track", "No available seats in this track");

        // build everything in memory
        var now = DateTime.UtcNow;

        intern.Status = "Accepted";

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
            Amount = track.Fees,
            Status = PaymentStatus.Completed,
            Method = request.PaymentMethod.Value,
            PaymentDate = now
        };

        enrollmentRepository.Add(enrollment);
        paymentRepository.Add(payment);

        // one SaveChanges = one transaction (all or nothing)
        await unitOfWork.SaveChangesAsync();

        return RequestResponse<int>.OK(enrollment.Id, 201);
    }

    private static RequestResponse<int> Fail(int code, string key, string message)
        => RequestResponse<int>.Fail(code, new Dictionary<string, string[]> { { key, [message] } });
}