using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands.HandlerCommands;

public class CreateTrackCommandHandler(IGeneralRepository<Track> _trackRepo) 
    : IRequestHandler<CreateTrackCommand, TrackDto>
{  
    public async Task<TrackDto> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {

        var track = new Track
        {
            Name = request.name,
            Fees = request.fees,
            IsActive = request.isActive,
            MaxCapacity = request.maxCapacity
        };

         _trackRepo.Add(track);
        return track.ToDto();

    }
}
