using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;

        public CreateTrackCommandHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<RequestResponse<int>> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = new Track
            {
                Name = request.Name,
                Fees = request.Fees,
                IsActive = request.IsActive,
                MaxCapacity = request.MaxCapacity
            };
            
            _trackRepository.Add(track);
            await _trackRepository.SaveChangesAsync();
            
            return RequestResponse<int>.Created(track.Id);
        }
    }
}
