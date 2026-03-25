using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using System.Diagnostics.Eventing.Reader;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class CheckTrackActivitionQueryHandler(IGeneralRepository<Track> _repository) : IRequestHandler<CheckTrackActivitionQuery, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(CheckTrackActivitionQuery request, CancellationToken cancellationToken)
        {
            var track =await _repository.GetById(request.trackId);

            if (track == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.TrackNotFound);
            }
            var isActive=track.IsActive;
            if (isActive)
            {
                return RequestResult<bool>.Success(isActive);
            }
            return RequestResult<bool>.Failure(ErrorCode.TrackNotActive);
            
        }
    }
}
