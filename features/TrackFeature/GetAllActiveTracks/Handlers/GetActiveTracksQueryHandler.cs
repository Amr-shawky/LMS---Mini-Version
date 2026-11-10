using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Handlers
{
    public class GetActiveTracksQueryHandler : IRequestHandler<GetActiveTracksQuery, RequestResponse<IEnumerable<ActiveTracksDTO>>>
    {
        private readonly IGeneralRepository<Track> repository;

        public GetActiveTracksQueryHandler(IGeneralRepository<Track> repository)
        {
            this.repository = repository;
        }
        public  async Task<RequestResponse<IEnumerable<ActiveTracksDTO>>> Handle(GetActiveTracksQuery request, CancellationToken cancellationToken)
        {
            var activeTracks = await  repository.GetAll().Where(x => x.IsActive).Select(x => new ActiveTracksDTO { 
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();

            if(activeTracks.Any())
            {
                return RequestResponse<IEnumerable<ActiveTracksDTO>>.OK(activeTracks, 200);
            }
            else
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
                    {
                        {"Failure", new[] { "There is not Active Tracks" }}

                    };
                return RequestResponse<IEnumerable<ActiveTracksDTO>>.Fail(400,errors);
            }


            

                

            
        }
    }
}
