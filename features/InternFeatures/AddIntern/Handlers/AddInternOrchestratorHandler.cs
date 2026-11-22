using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.AddIntern.Orchestrator;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.AddIntern.Handlers
{
    public class AddInternOrchestratorHandler(IMediator mediator) : IRequestHandler<AddInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator mediator = mediator;
       

        public async Task<RequestResponse<int>> Handle(AddInternOrchestrator request, CancellationToken cancellationToken)
        {
            var track = await mediator.Send(new GetTrackByIdQuery(request.TrackId));

            if (!track.Success)
            {
                return RequestResponse<int>.Fail(track.StatusCode, track.Errors);

            }

            var result = await mediator.Send(new AddInternCommand(request.FullName, request.Email, request.BirthYear, request.TrackId));
            if (!result.Success)
            {
                return RequestResponse<int>.Fail(result.StatusCode, result.Errors);
            }
           
            return result;
        }
    }
}
