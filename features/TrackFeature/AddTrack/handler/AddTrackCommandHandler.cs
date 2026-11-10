using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.AddTrack.handler
{
    public class AddTrackCommandHandler : IRequestHandler<AddTrackCommand, RequestResponse>
    {
        private readonly IGenericRepository<Track> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTrackCommandHandler(IGenericRepository<Track> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(AddTrackCommand request, CancellationToken cancellationToken)
        {
            var track = new Track { Name = request.Name, Fees = request.Fees, MaxCapacity = request.MaxCapacity };

            _repository.Add(track);

            //await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
