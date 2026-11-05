using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.TrackFeature.Handlers
{
    public class UpdateTrackFeesCommandHandler : IRequestHandler<UpdateTrackFeesCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Track> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTrackFeesCommandHandler(IGeneralRepository<Track> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(UpdateTrackFeesCommand request, CancellationToken cancellationToken)
        {
            var track =await _repository.GetAll().FirstOrDefaultAsync(x=> x.Id == request.ID);

            if (track==null)
            {
               // return RequestResponse.Fail(500);
            }
            
            track.Fees = request.fees;

            _repository.Update(track);

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK(201);
        }
    }
}
