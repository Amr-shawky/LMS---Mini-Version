using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Commands.HandlerCommands
{
    public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, TrackDto>
    {
        IGeneralRepository<Domain.Entities.Track> _trackRepo;
        public CreateTrackCommandHandler(IUnitOfWork unitOfWork, IGeneralRepository<Domain.Entities.Track> trackRepo)
        {
            _trackRepo = trackRepo;
        }
        public async Task<TrackDto> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {

            var track = new Domain.Entities.Track
            {
                Name = request.name,
                Fees = request.fees,
                IsActive = request.isActive,
                MaxCapacity = request.maxCapacity
            };

             _trackRepo.Add(track);
            return track.ToDto();

        }
    }
}
