using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, Unit>
    {
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTrackCommandHandler(IGeneralRepository<Track> trackRepo,IUnitOfWork unitOfWork)
        {
            _trackRepo = trackRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepo.GetByIdAsync(request.Id);
            if (track is null)
                return Unit.Value;
            track.Name=request.Name;
            track.Fees=request.Fees;
            track.IsActive=request.IsActive;
            track.MaxCapacity=request.MaxCapacity;

            _trackRepo.Update(track);
           await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
