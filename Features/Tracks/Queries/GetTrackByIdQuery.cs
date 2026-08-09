using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    public record GetTrackByIdQuery(int Id) : IRequest<TrackDto>;


    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackDto>
    {
        private readonly IGeneralRepository<Track> _trackrepository;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> trackrepository)
        {
            _trackrepository = trackrepository;
        }

        public async Task<TrackDto> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track =  await _trackrepository.GetByIdAsync(request.Id);
            return track.ToDto();
        }
    }
}
