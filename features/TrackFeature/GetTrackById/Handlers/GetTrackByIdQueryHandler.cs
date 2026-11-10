using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Handlers
{
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, RequestResponse<TrackByIdDTO>>
    {
        private readonly IGeneralRepository<Track> trackRepo;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepo)
        {
            this.trackRepo = trackRepo;
        }
        public async Task<RequestResponse<TrackByIdDTO>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await trackRepo.GetByIdAsync(request.trackId);
            if(track == null)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>()
                {
                    {"Track" , new[]{"This Track isnot Exists"} }
                };
                return RequestResponse<TrackByIdDTO>.Fail(404, errors);
            }
            else
            {
                var trackdto = new TrackByIdDTO()
                {
                    Id = track.Id,
                    TrackName = track.Name,
                    Fees = track.Fees,
                    IsActive = track.IsActive,
                };
                return    RequestResponse<TrackByIdDTO>.OK(trackdto,200);
            }
        }
    }
}
