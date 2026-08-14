using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        
        private readonly IUnitOfWork _unitOfWork;
        public CreateTrackCommandHandler(IGeneralRepository<Track> trackRepository, IUnitOfWork unitOfWork)
        {
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = new Track
            {
                Name = request.Name,
                Fees = request.Fees,
                IsActive = request.IsActive,
                MaxCapacity = request.MaxCapacity
            };
            
            _trackRepository.Add(track);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
