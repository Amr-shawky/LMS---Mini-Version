using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class cancelEnrollmentOrchestratorHandler : IRequestHandler<cancelEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public cancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(cancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // step 1 check if enrollment exists
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentID), cancellationToken);

            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
            {
                return RequestResponse.Fail($"Enrollment not found {request.enrollmentID}", 404);
            }

            var enrollment = enrollmentResponse.Data;
           
            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return RequestResponse.Fail("Enrollment is already cancelled", 400);
            }

            // step 2 & 3 execute updates within transaction
            await _unitOfWork.ExecuteAsync(async ct =>
            {
                await _mediator.Send(new updateEnrollmentStatusCommand(request.enrollmentID, EnrollmentStatus.Cancelled), ct);
                var refundResponse = await _mediator.Send(new refundPaymentCommand(request.enrollmentID), ct);
                if (!refundResponse.Success)
                    throw new InvalidOperationException(refundResponse.Message);

                if (refundResponse.Data.HasValue)
                {
                    var historyResponse = await _mediator.Send(new CreateRefundedPaymentHistoryCommand(request.enrollmentID, refundResponse.Data.Value), ct);
                    if (!historyResponse.Success)
                        throw new InvalidOperationException(historyResponse.Message);
                }
            },cancellationToken);

            return RequestResponse.Ok("Enrollment cancelled successfully");
        }
    }
}
