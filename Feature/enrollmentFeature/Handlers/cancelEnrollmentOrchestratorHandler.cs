using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class cancelEnrollmentOrchestratorHandler : IRequestHandler<cancelEnrollmentOrchestrator, Unit>
    {
        private readonly IMediator _mediator;

        public cancelEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Unit> Handle(cancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // step 1 check is the enrollemnt exist
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentID));
            
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"enrollment not found {request.enrollmentID}");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                throw new InvalidOperationException($"enrollment is already cancelled ");
                
            }

            // step 2 update enrollment status to cancelled 

            await _mediator.Send(new updateEnrollmentStatusCommand(request.enrollmentID ,EnrollmentStatus.Cancelled));


            // step 3 refund payment 
            await _mediator.Send(new refundPaymentCommand(request.enrollmentID));


            return Unit.Value;
        }
    }
}
