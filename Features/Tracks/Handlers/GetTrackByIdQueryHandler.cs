using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers;

public class GetTrackByIdQueryHandler(IGeneralRepository<Track> _trackRepository) : IRequestHandler<GetTrackByIdQuery, TrackDto?>
{
    
    public async Task<TrackDto?> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.Id);
        if(track == null) return  null;
        
        TrackDto trackDto=new  TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            Fees = track.Fees,
            IsActive = track.IsActive,
            MaxCapacity = track.MaxCapacity
            
        };

        return trackDto;   
    }
}
