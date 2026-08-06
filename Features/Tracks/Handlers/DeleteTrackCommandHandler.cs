using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand>
    {
        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IUnitOfWork _uow;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepo, IUnitOfWork uow)
        {
            _trackRepo = trackRepo;
            _uow = uow;
        }

        public async Task<Unit> Handle( DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepo.GetByIdAsync(request.Id);

            if (track is null)
                throw new Exception($"Track with Id {request.Id} not found");

            _trackRepo.Delete(track);

            await _uow.CompleteAsync();

            return Unit.Value;

        }


    }
}
