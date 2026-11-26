using LMS___Mini_Version.Domain.Constants;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Rules;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.handler
{
    /// <summary>
    /// One business action: accept the intern, open an Active enrollment on their assigned track,
    /// and record the Completed payment the admin already received.
    /// All three changes are staged in the change tracker and committed by a single
    /// IUnitOfWork.SaveChangesAsync() call, which EF Core runs inside one database transaction:
    /// either everything is saved or nothing is.
    /// </summary>
    public class ApproveAndRegisterInternCommandHandler
        : IRequestHandler<ApproveAndRegisterInternCommand, RequestResponse<InternRegistrationDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveAndRegisterInternCommandHandler(
            IGeneralRepository<Intern> internRepository,
            IGeneralRepository<Track> trackRepository,
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _trackRepository = trackRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<InternRegistrationDto>> Handle(
            ApproveAndRegisterInternCommand request, CancellationToken cancellationToken)
        {
            // 1. Input
            if (request.PaymentMethod is null || !Enum.IsDefined(request.PaymentMethod.Value))
                return Fail(StatusCodes.Status400BadRequest, nameof(request.PaymentMethod),
                    $"PaymentMethod is required and must be one of: {string.Join(", ", Enum.GetNames<PaymentMethod>())}.");

            // 2. Intern must exist and still be awaiting review
            var intern = await _internRepository.GetAll()
                .FirstOrDefaultAsync(i => i.Id == request.InternId, cancellationToken);

            if (intern is null)
                return Fail(StatusCodes.Status404NotFound, nameof(request.InternId),
                    $"Intern with id {request.InternId} was not found.");

            if (!InternStatuses.IsPending(intern.Status))
                return Fail(StatusCodes.Status409Conflict, "InternStatus",
                    $"Only Pending applicants can be approved. Intern '{intern.FullName}' is currently '{intern.Status}'.");

            // 3. The intern's assigned track must exist and be active
            var track = await _trackRepository.GetAll()
                .FirstOrDefaultAsync(t => t.Id == intern.TrackId, cancellationToken);

            if (track is null)
                return Fail(StatusCodes.Status409Conflict, "TrackId",
                    $"The track assigned to this intern (id {intern.TrackId}) no longer exists.");

            if (!track.IsActive)
                return Fail(StatusCodes.Status409Conflict, "TrackId",
                    $"Track '{track.Name}' is not active; registrations are closed.");

            var trackEnrollments = _enrollmentRepository.GetAll().Where(e => e.TrackId == track.Id);

            // 4. No second open (Pending/Active) enrollment for the same intern and track
            var hasOpenEnrollment = await trackEnrollments
                .Where(e => e.InternId == intern.Id)
                .AnyAsync(SeatReservationRules.ReservesSeat, cancellationToken);

            if (hasOpenEnrollment)
                return Fail(StatusCodes.Status409Conflict, "Enrollment",
                    $"Intern '{intern.FullName}' already has a Pending or Active enrollment in '{track.Name}'.");

            // 5. A seat must be available (same rule as GET /api/Track/{id}/availability)
            var reservedSeats = await trackEnrollments.CountAsync(SeatReservationRules.ReservesSeat, cancellationToken);
            var availableSeats = SeatReservationRules.CalculateAvailableSeats(track.MaxCapacity, reservedSeats);

            if (availableSeats <= 0)
                return Fail(StatusCodes.Status409Conflict, "TrackId",
                    $"Track '{track.Name}' is full ({reservedSeats}/{track.MaxCapacity} seats reserved).");

            // 6. Stage all business changes, then commit once
            var now = DateTime.UtcNow;

            intern.Status = InternStatuses.Accepted;

            var enrollment = new Enrollment
            {
                InternId = intern.Id,
                TrackId = track.Id,
                EnrollmentDate = now,
                Status = EnrollmentStatus.Active,
                Payment = new Payment
                {
                    Amount = track.Fees,
                    PaymentDate = now,
                    Method = request.PaymentMethod.Value,
                    Status = PaymentStatus.Completed
                }
            };

            _enrollmentRepository.Add(enrollment);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // The transaction was rolled back, so no partial registration exists.
                return Fail(StatusCodes.Status409Conflict, "Registration",
                    "The registration could not be saved. No changes were applied; please try again.");
            }

            var payment = enrollment.Payment!;
            return RequestResponse<InternRegistrationDto>.OK(new InternRegistrationDto
            {
                EnrollmentId = enrollment.Id,
                InternId = intern.Id,
                InternStatus = intern.Status,
                TrackId = track.Id,
                TrackName = track.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                EnrollmentStatus = enrollment.Status,
                PaymentId = payment.Id,
                PaymentAmount = payment.Amount,
                PaymentMethod = payment.Method,
                PaymentStatus = payment.Status,
                PaymentDate = payment.PaymentDate
            }, StatusCodes.Status201Created);
        }

        private static RequestResponse<InternRegistrationDto> Fail(int statusCode, string field, string error)
            => RequestResponse<InternRegistrationDto>.Fail(statusCode, field, error);
    }
}
