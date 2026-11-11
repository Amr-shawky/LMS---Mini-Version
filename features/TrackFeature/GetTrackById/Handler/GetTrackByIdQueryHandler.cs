using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Handler
{
    public class GetTrackByIdQueryHandler
       : IRequestHandler<GetTrackByIdQuery, RequestResponse<TrackDtoById>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<TrackDtoById>> Handle(
            GetTrackByIdQuery request,
            CancellationToken cancellationToken)
        {
            var track = await _repository.GetByIdAsync(request.Id);

            if (track is null)
            {
                return RequestResponse<TrackDtoById>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track not found." }
                    });
            }

            var dto = new TrackDtoById(
                track.Id,
                track.Name,
                track.IsActive
            );

            return RequestResponse<TrackDtoById>.OK(dto);
        }
    }
}
