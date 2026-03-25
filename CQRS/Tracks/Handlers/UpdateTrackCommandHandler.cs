using LMS___Mini_Version.CQRS.Tracks.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class UpdateTrackCommandHandler(IGeneralRepository<Track> _repository) : IRequestHandler<UpdateTrackCommand,RequestResult< TrackDto>>
    {
        public async Task<RequestResult<TrackDto>> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetById(request.trackId);
            if (track is null)
            {
                return RequestResult<TrackDto>.Failure(ErrorCode.TrackNotFound);
            }
            track.Name = request.Name;
            track.Fees = request.Fees;
            track.IsActive = request.IsActive;
            track.MaxCapacity = request.MaxCapcity;
            _repository.Update(track);
            return RequestResult<TrackDto>.Success(track.toTrackDto());

        }
    }
}
