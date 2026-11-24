using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.features.TrackFeature.Orchestrators.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.Orchestrators
{
    public record TrackAvailabilityOrchestrator(int trackId): IRequest<RequestResponse<TrackAvailabilityDto>>
    {
    }

    public class TrackAvailabilityOrchestratorHandler : IRequestHandler<TrackAvailabilityOrchestrator, RequestResponse<TrackAvailabilityDto>>
    {
        private readonly IMediator _mediator;

        public TrackAvailabilityOrchestratorHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<RequestResponse<TrackAvailabilityDto>> Handle(TrackAvailabilityOrchestrator request, CancellationToken cancellationToken)
        {
            if(request.trackId <= 0)
                return RequestResponse<TrackAvailabilityDto>.Fail(ErrorCode.InvalidTrackId);

            // query to get track by id => do u have to check?
            var track = await _mediator.Send(new GetTrackByIdQuery(request.trackId), cancellationToken);
            if(!track.Success)
                return RequestResponse<TrackAvailabilityDto>.Fail(ErrorCode.InvalidTrackId);


            // query to get all enrollments status in track
            var enrollments = await _mediator.Send(new GetEnrollmentsByTrackQuery(request.trackId), cancellationToken);

            var data = enrollments.Data;
            int reserved = 0, available = track.Data!.MaxCapacity;
            if (data != null) {
                
                foreach (var enrollment in data)
                {
                    if (enrollment.EnrollmentStatus is "Pending" or "Active")
                        reserved++;
                }
                available = track.Data!.MaxCapacity >= reserved ? 
                    track.Data.MaxCapacity - reserved : 0;

                

            }

            return RequestResponse<TrackAvailabilityDto>.OK(new TrackAvailabilityDto 
            { 
                TrackName = track.Data.Name,
                IsActive = track.Data.IsActive,
                MaxCapacity = track.Data.MaxCapacity,
                ReservedSeats = reserved,
                AvailabeSeats = available
            });

        }
    }
}
