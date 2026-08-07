using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, bool>
    {
        private readonly IGeneralRepository<Track> _trackRepostory;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTrackCommandHandler(IGeneralRepository<Track> trackRepostory, IUnitOfWork unitOfWork)
        {
            _trackRepostory = trackRepostory;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepostory.GetTable()
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (track == null)
                return false;
            track.Name = request.Name;
            track.Fees = request.Fees;
            track.IsActive = request.IsActive;
            track.MaxCapacity = request.MaxCapacity;

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
