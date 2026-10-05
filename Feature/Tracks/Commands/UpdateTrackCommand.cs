using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS___Mini_Version.Feature.Tracks.Commands
{
    public record UpdateTrackCommand(int? Id , string? Name , decimal? Fees , bool? IsActive , int? MaxCapacity) : IRequest<RequestResponse>;

    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, RequestResponse> 
    {
        private readonly IGeneralRepository<Track> _trackrepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTrackCommandHandler(IGeneralRepository<Track> trackrepository,
            IUnitOfWork unitOfWork)
        {
            _trackrepository = trackrepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {

            Track track = new Track { Id = request.Id.Value, Fees = request.Fees.Value };


            _trackrepository.SaveInclude(track,nameof(Track.Fees));


            await _unitOfWork.SaveChangesAsync();
            return RequestResponse.Ok();
        } 
            #region update 
            //var entity = await _trackrepository.GetByIdAsync(request.Id.Value);
            //var entity = new Track {Id = request.Id.Value,
            //Fees = request.Fees.Value
            //};

            //entity.Fees = request.Fees.Value;

            //_trackrepository.Update(entity);

            //await _unitOfWork.SaveChangesAsync();

            #endregion

            #region executepdate 
            //var entity = await _trackrepository.GetAll()
            //    .Where(t => t.Id == request.Id.Value)
            //    .ExecuteUpdateAsync(setters => 
            //    setters.SetProperty(d => d.Fees ,request.Fees.Value));

            #endregion

            #region save include 
            //var track = new Track
            //{
            //    Id = request.Id.Value,
            //    Name = request.Name,
            //    IsActive = request.IsActive.Value,
            //    Fees = request.Fees.Value,
            //    MaxCapacity = request.MaxCapacity.Value
            //};
            //if (track == null)
            //    return RequestResponse.Fail("Track not found");

            //if (!string.IsNullOrWhiteSpace(request.Name))
            //    track.Name = request.Name;


            //if (request.Fees.HasValue)
            //    track.Fees = request.Fees.Value;


            //if (request.IsActive.HasValue)
            //    track.IsActive = request.IsActive.Value;

            //if (request.MaxCapacity.HasValue)
            //    track.MaxCapacity = request.MaxCapacity.Value;


            //await _trackrepository.SaveIncludeAsync(track, nameof(Track.Fees));
            //await _unitOfWork.SaveChangesAsync();

            #endregion
    }

}



