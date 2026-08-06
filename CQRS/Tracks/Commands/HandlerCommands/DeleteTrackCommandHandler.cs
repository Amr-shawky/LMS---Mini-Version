using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Commands.HandlerCommands
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, bool>
    {

        IGeneralRepository<Domain.Entities.Track> _trackRepo;
        public DeleteTrackCommandHandler(IGeneralRepository<Domain.Entities.Track> trackRepo)
        {
            _trackRepo = trackRepo;
        }
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
}
