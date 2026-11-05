using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Handlers
{
    public class CreateTrackCommandHandler:IRequestHandler<CreateTrackCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Track> _repository;

        private readonly IUnitOfWork _unitOfWork;
        public CreateTrackCommandHandler(IGeneralRepository<Track> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(CreateTrackCommand request,CancellationToken cancellation)
        {
            var track = new Track
            {
                Name = request.name,
                Fees = request.Fees,
                MaxCapacity = request.MaxCapacity
            };

            _repository.Add(track);

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK(201);

        }
    }
}
