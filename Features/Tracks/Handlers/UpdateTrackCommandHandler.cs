using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;
using System.Runtime.InteropServices;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{

    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, Unit>
    {

        private readonly IGeneralRepository<Track> _trackRepo;
        private readonly IUnitOfWork _uow;

        public UpdateTrackCommandHandler(IGeneralRepository<Track> trackRepo, IUnitOfWork uow)
        {
            _trackRepo = trackRepo;
            _uow = uow;
        }
        public  async Task<Unit> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var isFound =  _trackRepo.GetTable().Any(t => t.Id == request.id);

            if (!isFound)
                throw new Exception("Track Not Found To Update");

            var track = new Track{
                Id = request.id,
                Name = request.name,
                Fees = request.fees,
                MaxCapacity = request.MaxCapacity,
                IsActive = request.IsActive
            };
    
            _trackRepo.Update(track);
            await _uow.CompleteAsync();

            return Unit.Value;

        }

    }


}
