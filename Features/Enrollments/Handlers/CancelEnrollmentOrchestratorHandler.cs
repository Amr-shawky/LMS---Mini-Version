using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Orchestrators;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class CancelEnrollmentOrchestratorHandler : IRequestHandler<CancelEnrollmentOrchestrator>
    {
        private readonly IMediator _mediator;

        public CancelEnrollmentOrchestratorHandler(IMediator mediator)
          => _mediator = mediator;

        public async Task<Unit> Handle(CancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetEnrollmentsByIdQuery(request.EnrollmentId));
            await _mediator.Send(new UpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled));
            await _mediator.Send(new RefundPaymentCommand(request.EnrollmentId));
            return Unit.Value;
            //1:52
        }
    }
}
