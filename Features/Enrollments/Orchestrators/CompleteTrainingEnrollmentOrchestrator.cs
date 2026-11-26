using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public record CompleteTrainingEnrollmentOrchestrator(int enrollmentId) : IRequest<RequestResponse>;

    public class CompleteTrainingEnrollmentOrchestratorHandler : IRequestHandler<CompleteTrainingEnrollmentOrchestrator,RequestResponse>
    {
        private readonly IMediator _mediator;

        public CompleteTrainingEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse> Handle(CompleteTrainingEnrollmentOrchestrator orchestrator,CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentForCompletationQuey(orchestrator.enrollmentId), cancellationToken);
            if (!enrollment.Success)
                return RequestResponse.Fail(404, null, "Enrollment is Not Found");

            var enrollmentStatus = enrollment.Data.Status;
            if (enrollmentStatus != EnrollmentStatus.Active)
                return RequestResponse.Fail(404, null, "Enrollment is not Active");


            var payment = await _mediator.Send(new CheckCompletedPaymentQuery(orchestrator.enrollmentId), cancellationToken);
            if(!payment.Success)
                return RequestResponse.Fail(404,null,"Payment is Not Found");

            var paymentStatus = payment.Data.Status;
            if (paymentStatus != PaymentStatus.Completed)
                return RequestResponse.Fail(404, null, "Payment is not Completed");

            var CompleteAction = await _mediator.Send(new MarkEnrollmentCompletedCommand(orchestrator.enrollmentId), cancellationToken);

            if (!CompleteAction.Success)
                return RequestResponse.Fail(409, null, "Can't Update the Enrollment Status");

            return RequestResponse.OK("Completed Successfully");


        }
    }
}