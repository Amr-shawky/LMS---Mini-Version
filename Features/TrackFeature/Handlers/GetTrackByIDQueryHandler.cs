using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Dtos;
using LMS___Mini_Version.Features.TrackFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Handlers
{
    public class GetTrackByIDQueryHandler : IRequestHandler<GetTrackByIDQuery,RequestResponse<TrackDto>>
    {
        private readonly IGeneralRepository<Track> _repository;
         public GetTrackByIDQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<TrackDto>> Handle(GetTrackByIDQuery request,CancellationToken token )
        {
            var track =await _repository.GetByIdAsync(request.id);

            if (track == null)
            {
                IDictionary<string, string[]>? Errors = null;
                return RequestResponse<TrackDto>.Fail(500, Errors);
            }

            if (!track.IsActive)
            {
                IDictionary<string, string[]>? Errors = null;
                return RequestResponse<TrackDto>.Fail(500, Errors);
            }

            TrackDto trackDto = new TrackDto
            {
                id = track.Id,
                Fees = track.Fees,
                MaxCapacity = track.MaxCapacity,
                Name = track.Name
            };

            return RequestResponse<TrackDto>.OK(200,trackDto);
        }
    }
}
