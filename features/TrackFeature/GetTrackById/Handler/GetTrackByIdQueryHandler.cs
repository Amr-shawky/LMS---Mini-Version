using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.DTOs;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Handler
{
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, RequestResponse<GetTrackByIdDto>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<GetTrackByIdDto>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var track = await _repository.GetByIdAsync(request.id);
                if (track == null)
                {
                    return RequestResponse<GetTrackByIdDto>.Fail();
                }
                var trackDto = new GetTrackByIdDto
                {
                    id = track.Id,
                    name = track.Name,
                    IsActive = track.IsActive,
                    MaxCapacity = track.MaxCapacity
                };

                return RequestResponse<GetTrackByIdDto>.OK(trackDto);
            }
            catch (Exception ex)
            {
                return RequestResponse<GetTrackByIdDto>.Fail();
            }
        }
    }
}
