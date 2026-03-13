using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Commands.HandlerCommands;

public class UpdateTrackCommandHandler(IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<UpdateTrackCommand, bool>
{   
    public async Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {

        var track = await _trackRepo.GetTable().Where(t => t.Id == request.id).FirstOrDefaultAsync(cancellationToken);
        if (track == null) { return false; }
        
        track.Name = request.name;
        track.Fees = request.fees;
        track.IsActive = request.isActive;
        track.MaxCapacity = request.maxCapacity;
        _trackRepo.Update(track);

        return true;
    }
}
