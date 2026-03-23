using LMS___Mini_Version.CQRS.Tracks.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using System.Diagnostics;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class CreateTrackCommandHandler(IGeneralRepository<Track> _repository) : IRequestHandler<CreateTrackCommand, Track>
    {
        public Task<Track> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = new Track()
            {
                Name=request.Name,
                Fees=request.Fees,
                IsActive=request.IsActive,
                MaxCapacity=request.MaxCapcity
            };
            _repository.Add(track);
            return Task.FromResult(track);
        }
    }
}
