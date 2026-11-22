using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.handler
{
    public class GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepo) : IRequestHandler<GetTrackByIdQuery, RequestResponse<TrackDto>>
    {
        private readonly IGeneralRepository<Track> trackRepo = trackRepo;

        public async Task<RequestResponse<TrackDto>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await trackRepo.GetByIdAsync(request.TrackId);

            if (track == null)
            {
                return RequestResponse<TrackDto>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.TrackId)] = new[] { "Track not found" }
                });
            }

            if (!track.IsActive)
            {
                return RequestResponse<TrackDto>.Fail(404, new Dictionary<string, string[]>
                {
                    [nameof(request.TrackId)] = new[] { "Track is not Active" }
                });
            }
          

            var TrackDto = new TrackDto
            {
                 id = track.Id,
                 IsActive = track.IsActive,
                 name = track.Name,
                 MaxCapacity = track.MaxCapacity,             
            };
            return RequestResponse<TrackDto>.OK(TrackDto);

        }
    }
}
