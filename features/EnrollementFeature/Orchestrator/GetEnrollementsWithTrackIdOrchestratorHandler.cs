using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Orchestrator
{
    public class GetEnrollementsWithTrackIdOrchestratorHandler : IRequestHandler<GetEnrollementsWithTrackIdOrchestrator, RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>>
    {
        private readonly IMediator mediator;

        public GetEnrollementsWithTrackIdOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>> Handle(GetEnrollementsWithTrackIdOrchestrator request, CancellationToken cancellationToken)
        {
            var trackResponse = await mediator.Send(new GetTrackByIdQuery(request.trackId));
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>.Fail(trackResponse.StatusCode, trackResponse.Errors);
            }
            else
            {
                var response = await  mediator.Send(new GetEnrollementWithTrackIdQuery(request.trackId));
                return response;
            }
        }
    }
}
