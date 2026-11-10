using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetAllTracks.handler
{
    public class GetAllTracksQueryHandler : IRequestHandler<GetAllTracksQuery, RequestResponse<IEnumerable<TrackDto>>>
    {
        private readonly IGenericRepository<Domain.Entities.Track> _repository;
        public GetAllTracksQueryHandler(IGenericRepository<Domain.Entities.Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<TrackDto>>> Handle(GetAllTracksQuery request, CancellationToken cancellationToken)
        {

            var tracks = await _repository.GetAll()
                .Skip((request.pageindex - 1) * request.pagesize)
                .Take(request.pagesize)
                .ToListAsync();


            var tracksdto = tracks.Select(x =>
            new TrackDto
            {

                Id = x.Id,
                Name = x.Name,
                MaxCapacity = x.MaxCapacity,
                Totalenrollment = x.Enrollments.Count()

            });

            return RequestResponse<IEnumerable<TrackDto>>.OK(tracksdto);

        }

    }
}
