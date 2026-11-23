using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Orchestrator
{
    public class GetEnrollementWithInternIdOrchestratorHandler : IRequestHandler<GetEnrollementWithInternIdOrchestrator, RequestResponse<IEnumerable<EnrollementByInternIdDTO>>>
    {
        private readonly IMediator mediator;

        public GetEnrollementWithInternIdOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public  async Task<RequestResponse<IEnumerable<EnrollementByInternIdDTO>>> Handle(GetEnrollementWithInternIdOrchestrator request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetInternByIdQuery(request.InternId));
            if (!response.Success || response.Data is null)
                return RequestResponse<IEnumerable<EnrollementByInternIdDTO>>.Fail(response.StatusCode, response.Errors);

            else
            {
                var res = await mediator.Send(new GetEnrollementWithInternIdQuery(request.InternId));
                return res;
            }
        }

       
    }
}
