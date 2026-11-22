using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrackExists
{
    public record CheckTrackExistsQuery(int TrackId): IRequest<RequestResponse<bool>>
    {
    }

    public class CheckTrackExistsQueryHandler : IRequestHandler<CheckTrackExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Track> _repository;

        public CheckTrackExistsQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<bool>> Handle(CheckTrackExistsQuery request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.TrackId);

            return RequestResponse<bool>.OK(exists);
        }
    }
}
