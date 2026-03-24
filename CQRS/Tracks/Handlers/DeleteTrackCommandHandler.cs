using LMS___Mini_Version.CQRS.Tracks.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class DeleteTrackCommandHandler(IGeneralRepository<Track> _repository) : IRequestHandler<DeleteTrackCommand, bool>
    {
        public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track=await _repository.GetById(request.id);
            if (track is null)
                return false;
            _repository.Delete(track.Id);
            return true;
            
        }
    }
}
