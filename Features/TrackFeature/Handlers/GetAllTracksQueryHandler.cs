using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Dtos;
using LMS___Mini_Version.Features.TrackFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.TrackFeature.Handlers
{
    public class GetAllTracksQueryHandler : IRequestHandler<GetAllTracksQuery, RequestResponse<IEnumerable<TrackDto>>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public GetAllTracksQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<TrackDto>>> Handle(GetAllTracksQuery request, CancellationToken cancellationToken)
        {

            
            var tracks = await _repository.GetAll()
                .Skip((request.pageindex-1) * request.pagesize)
                .Take(request.pagesize)
                .ToListAsync();


            var trackdto = tracks.Select(x=>new TrackDto
            {
                id = x.Id,
                Fees = x.Fees,
                MaxCapacity = x.MaxCapacity,
                Name = x.Name
            });
            var requestresponse = RequestResponse<IEnumerable<TrackDto>>.OK(200, trackdto);

            return requestresponse;

        }
    }
}
