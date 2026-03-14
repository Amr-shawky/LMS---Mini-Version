using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using MediatR;
using System.Net;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class CancelEnrollmentOrchestratorHandler : IRequestHandler<CancelEnrollmentOrchestratorRequest, CommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(CancelEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {

            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null)
                return CommandResult.Fail($"Enrollment with ID {request.EnrollmentId} not found.");

            if (enrollment.Status == EnrollmentStatus.Cancelled.ToString())
                return CommandResult.Fail($"Enrollment with ID {request.EnrollmentId} is already cancelled.");

            var statusUpdated = await _mediator.Send(new StageUpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);
            if (!statusUpdated)
                return CommandResult.Fail($"Failed to update status for enrollment with ID {request.EnrollmentId}.");

            await _mediator.Send(new StageRefundPaymentCommand(request.EnrollmentId), cancellationToken);

            await _unitOfWork.CompleteAsync();

            return CommandResult.Succeed(
                "Enrollment cancelled and payment refunded successfully.");
        }
    }
}
