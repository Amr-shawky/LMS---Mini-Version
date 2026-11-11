using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Handlers
{
    public class GetActiveTracksQueryHandler : IRequestHandler<GetActiveTracksQuery, RequestResponse<IEnumerable<ActiveTracksDTO>>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public GetActiveTracksQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<ActiveTracksDTO>>> Handle(GetActiveTracksQuery request, CancellationToken cancellationToken)
        {
            var activeTracks = await _repository.GetAll()
                .Where(t => t.IsActive)
                .Select(t => new ActiveTracksDTO
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
            if(activeTracks is null || !activeTracks.Any())
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
                    {
                        {"Failure", new[] { "There is not Active Tracks" }}

                    };
                return RequestResponse<IEnumerable<ActiveTracksDTO>>.Fail(400, errors);
            }
            return RequestResponse<IEnumerable<ActiveTracksDTO>>.OK(activeTracks, 200);
        }
    }
}
