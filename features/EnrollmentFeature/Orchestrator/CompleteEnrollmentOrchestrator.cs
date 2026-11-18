using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Orchestrator
{
    public record CompleteEnrollmentOrchestrator(int id): IRequest<RequestResponse>
    {
    }

    public class CompleteEnrollmentOrchestratorHandler : IRequestHandler<CompleteEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;

        public CompleteEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse> Handle(CompleteEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                return RequestResponse<EnrollmentDto>.Fail(ErrorCode.InvalidEnrollmentId);

            var enrollment = await _mediator.Send(new GetEnrollmentQuery(request.id), cancellationToken);

            if (enrollment.Data!.EnrollmentStatus != EnrollmentStatus.Active)
                return RequestResponse.Fail(ErrorCode.EnrollmentNotActive, 400);

      
            if (enrollment.Data.PaymentStatus != "Completed")
                return RequestResponse<EnrollmentDto>.Fail(ErrorCode.PaymentNotCompleted, 409);

            return await _mediator.Send(new CompleteEnrollmentCommand(request.id), cancellationToken);

        }
    }
}
