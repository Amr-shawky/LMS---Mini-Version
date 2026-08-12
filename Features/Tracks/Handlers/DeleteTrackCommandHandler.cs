using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, Unit>
    {
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepo, IUnitOfWork unitOfWork)
        {
            _trackRepo = trackRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track =await _trackRepo.GetByIdAsync(request.id);
            if (track is null)
                return Unit.Value;
            _trackRepo.Delete(track);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
