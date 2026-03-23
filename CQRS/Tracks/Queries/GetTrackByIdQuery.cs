using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record GetTrackByIdQuery(int Id) : IRequest<TrackSummaryDTO>;
   
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackSummaryDTO>
    {
        private readonly IGeneralRepository<Track> _repository;
        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<TrackSummaryDTO> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetTable()
                         .Include(t=>t.Interns)
                         .Include(t=>t.Enrollments)
                         .FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (track == null)
            {
                throw new Exception($"Track with ID {request.Id} not found.");
            }

            return track.ToTrackSummaryDto();
        }
    }
}
