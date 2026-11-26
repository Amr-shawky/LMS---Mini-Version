using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.handler
{
    public class CompleteEnrollmentCommandHandler : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEnrollmentCommandHandler(
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            // The enrollment is loaded as a tracked entity; only the payment *status* is projected,
            // so the Payment row is never tracked and cannot be modified by this command.
            var row = await _enrollmentRepository.GetAll()
                .Where(e => e.Id == request.EnrollmentId)
                .Select(e => new
                {
                    Enrollment = e,
                    PaymentStatus = e.Payment == null ? (PaymentStatus?)null : e.Payment.Status
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (row is null)
                return RequestResponse.Fail(StatusCodes.Status404NotFound,
                    nameof(request.EnrollmentId), $"Enrollment with id {request.EnrollmentId} was not found.");

            var enrollment = row.Enrollment;

            if (enrollment.Status != EnrollmentStatus.Active)
            {
                var reason = enrollment.Status switch
                {
                    EnrollmentStatus.Completed => "This enrollment is already Completed.",
                    EnrollmentStatus.Cancelled => "A Cancelled enrollment cannot be completed.",
                    _ => $"Only Active enrollments can be completed. Current status is '{enrollment.Status}'."
                };
                return RequestResponse.Fail(StatusCodes.Status409Conflict, "EnrollmentStatus", reason);
            }

            if (row.PaymentStatus is null)
                return RequestResponse.Fail(StatusCodes.Status409Conflict, "Payment",
                    "No payment is recorded for this enrollment; it cannot be completed.");

            if (row.PaymentStatus != PaymentStatus.Completed)
                return RequestResponse.Fail(StatusCodes.Status409Conflict, "Payment",
                    $"The enrollment payment is '{row.PaymentStatus}'. Only enrollments with a Completed payment can be completed.");

            // Only the enrollment status changes. Intern, track assignment and payment stay untouched.
            // (Repository.Update() is deliberately not called: it would mark every column of the graph as modified.)
            enrollment.Status = EnrollmentStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
