using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackExists;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Orchestrator
{
    public record GetTrackEnrollmentsOrchestrator(int TrackId): IRequest<RequestResponse<IEnumerable<EnrollmentsForTrackDto>>>
    {
    }

    public class GetTrackEnrollmentsOrchestratorHandler : IRequestHandler<GetTrackEnrollmentsOrchestrator, RequestResponse<IEnumerable<EnrollmentsForTrackDto>>>
    {
        private readonly IMediator _mediator;

        public GetTrackEnrollmentsOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentsForTrackDto>>> Handle(GetTrackEnrollmentsOrchestrator request, CancellationToken cancellationToken)
        {
            if(request.TrackId <= 0)
                return RequestResponse<IEnumerable<EnrollmentsForTrackDto>>.Fail(ErrorCode.InvalidTrackId);

            var check = await _mediator.Send(new CheckTrackExistsQuery(request.TrackId), cancellationToken);

            if (check.Data == false)
                return RequestResponse<IEnumerable<EnrollmentsForTrackDto>>.Fail(ErrorCode.InvalidTrackId);

            return await _mediator.Send(new GetTrackEnrollmentsQuery(request.TrackId), cancellationToken);
        }
    }
}
