using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Commands.HandlerCommands;

public class DeleteTrackCommandHandler (IGeneralRepository<Track> _trackRepo)
    : IRequestHandler<DeleteTrackCommand, bool>
{  
    public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
    {
        var track = await _trackRepo.GetTable()
            .Where(t => t.Id == request.id)
            .FirstOrDefaultAsync(cancellationToken);
      
        if (track == null)
               return false;
        _trackRepo.Delete(track);

        return true;
    }
}
