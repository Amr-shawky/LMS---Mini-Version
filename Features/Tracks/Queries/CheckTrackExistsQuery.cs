using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    public record CheckTrackExistsQuery(int Id) : IRequest<bool>;


    public class CheckTrackExistsQueryHandler : IRequestHandler<CheckTrackExistsQuery, bool>
    {
        private readonly IGeneralRepository<Track> _trackrepository;

        public CheckTrackExistsQueryHandler(IGeneralRepository<Track> trackrepository)
        {
            _trackrepository = trackrepository;
        }
        public async Task<bool> Handle(CheckTrackExistsQuery request, CancellationToken cancellationToken)
        {
            return await _trackrepository.ExistsAsync(request.Id);
        }
    }
}
