using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{

    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackDto?>
    {

        private readonly IGeneralRepository<Track> _tracks;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> tracks)
        {
            _tracks = tracks;
        }


        public async Task<TrackDto> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _tracks.GetByIdAsync(request.id);

            return track?.ToDto();
        }
    }


   
}
