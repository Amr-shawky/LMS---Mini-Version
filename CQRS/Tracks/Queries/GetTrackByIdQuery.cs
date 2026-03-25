using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.TracksDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record GetTrackByIdQuery(int Id) : IRequest<RequestResult<TrackSummaryDTO?>>;
   
    public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, RequestResult<TrackSummaryDTO?>>
    {
        private readonly IGeneralRepository<Track> _repository;
        public GetTrackByIdQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResult<TrackSummaryDTO?>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetTable()
                         .Include(t=>t.Interns)
                         .Include(t=>t.Enrollments)
                         .FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (track == null)
            {
                return  RequestResult<TrackSummaryDTO?>.Failure(ErrorCode.TrackNotFound, $"the request with TrackId{request.Id} is not Founded");
                //throw new Exception($"Track with ID {request.Id} not found.");
            }

            return RequestResult<TrackSummaryDTO?>.Success(track.ToTrackSummaryDto());
        }
    }
}
