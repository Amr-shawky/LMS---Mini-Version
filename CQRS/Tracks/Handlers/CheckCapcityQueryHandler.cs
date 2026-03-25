using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class CheckCapcityQueryHandler(IGeneralRepository<Track> _repository) : IRequestHandler<CheckCapacityQuery, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(CheckCapacityQuery request, CancellationToken cancellationToken)
        {
            var track = await _repository.GetTable()
                       .Include(t => t.Enrollments)
                       .FirstOrDefaultAsync(t => t.Id == request.trackId, cancellationToken)
                       .ConfigureAwait(false);

            if (track == null) return RequestResult<bool>.Failure(ErrorCode.TrackNotFound);


            var activeCount = track.Enrollments
             .Count(e => e.Status != EnrollmentStatus.Cancelled);
            return RequestResult<bool>.Success(activeCount < track.MaxCapacity);


        }
    }
}
