using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Handlers;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Orchestrator
{
    public record EnrollmentHistoryOrchestrator(int internId): IRequest<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>
    {
    }

    public class EnrollmentHistoryOrchestratorHandler : IRequestHandler<EnrollmentHistoryOrchestrator, RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>
    {
        private readonly IMediator _mediator;

        public EnrollmentHistoryOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>> Handle(EnrollmentHistoryOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.internId <= 0)
                return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.Fail(ErrorCode.InvalidInternId);


            var intern = await _mediator.Send(new CheckInternExistsQuery(request.internId));
            if(intern.Data)
                return await _mediator.Send(new GetInternEnrollmentHistoryQuery(request.internId));

            return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.Fail(ErrorCode.InternNotCreated, 404);

        }
    }
}
