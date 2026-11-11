using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Queries;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetActiveTrackById.handler
{
    public class GetTrackByIdQueryHandler:IRequestHandler<GetTrackByIdQuery, RequestResponse<GetTrackByIdDto>>
    {
        private readonly IGeneralRepository<Track> _repository;
        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<GetTrackByIdDto>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetByIdAsync(request.Id);
            if (track == null)
            {
                return RequestResponse<GetTrackByIdDto>.Fail(404, new Dictionary<string, string[]>{{ "Track", new[] { "Track not found." } }});
            }
            return RequestResponse<GetTrackByIdDto>.OK(new GetTrackByIdDto
            {
                Name = track.Name,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                Fees = track.Fees
            });
        }
    }
}
