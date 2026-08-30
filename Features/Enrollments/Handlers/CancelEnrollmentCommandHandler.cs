using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
        {
            // ─── Step 1: Validate Enrollment ────────────────────────────────
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {request.EnrollmentId} was not found.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                throw new InvalidOperationException("This enrollment is already cancelled.");
            }

            // ─── Step 2: Update Enrollment Status to Cancelled ──────────────
            await _mediator.Send(new UpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);

            // ─── Step 3: Refund Associated Payment ──────────────────────────
            await _mediator.Send(new RefundPaymentCommand(request.EnrollmentId), cancellationToken);

            // ─── Step 4: Single Atomic Commit to Database ───────────────────
            await _unitOfWork.CompleteAsync();
        }
    }
}
