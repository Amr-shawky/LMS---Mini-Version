using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Domain.Entities;
using MediatR;
using LMS___Mini_Version.Feature.Tracks.Query;


namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class GetAllTrackQueryHandler : IRequestHandler<GetAllTrackQuery, IEnumerable<TrackDto>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        public GetAllTrackQueryHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }
        public Task<IEnumerable<TrackDto>> Handle(GetAllTrackQuery request, CancellationToken cancellationToken)
        {
            var tracks = _trackRepository.GetAll().Select(t => new TrackDto
            {
                Id = t.Id,
                Name = t.Name,
                Fees = t.Fees,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity
            }).ToList();

            return Task.FromResult<IEnumerable<TrackDto>>(tracks);
        }
    }
}
