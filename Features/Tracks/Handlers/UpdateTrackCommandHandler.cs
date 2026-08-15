using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand>
    {
        private readonly IGeneralRepository<Track> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTrackCommandHandler(IGeneralRepository<Track> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetByIdAsync(request.Id);
            if (track == null) return;

            track.Name = request.Name;
            track.Fees = request.Fees;
            track.IsActive = request.IsActive;
            track.MaxCapacity = request.MaxCapacity;

            _repository.Update(track);
            await _unitOfWork.CompleteAsync();
        }
    }
}