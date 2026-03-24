using LMS___Mini_Version.CQRS.Tracks.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class UpdateTrackCommandHandler(IGeneralRepository<Track> _repository) : IRequestHandler<UpdateTrackCommand, bool>
    {
        public async Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetById(request.trackId);
            if (track is null)
            {
                return false;
            }
            track.Name = request.Name;
            track.Fees = request.Fees;
            track.IsActive = request.IsActive;
            track.MaxCapacity = request.MaxCapcity;
            _repository.Update(track);
            return true;

        }
    }
}
