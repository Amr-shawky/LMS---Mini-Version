using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeneralRepository<Track> _trackRepo;

        public DeleteTrackCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Track> trackRepo)
        {
            _unitOfWork = unitOfWork;
            _trackRepo = trackRepo;
        }

        public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepo.GetByIdAsync(request.id);

            if (track == null) return false; 
            
            _trackRepo.Delete(track);
            await _unitOfWork.CompleteAsync();

            return true; 
        }
    }
}
