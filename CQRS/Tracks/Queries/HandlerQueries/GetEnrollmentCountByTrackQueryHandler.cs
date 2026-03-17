using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Tracks.Queries.HandlerQueries
{

    public class GetEnrollmentCountByTrackQueryHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<GetEnrollmentCountByTrackQuery, int>
    {
        public async Task<int> Handle(GetEnrollmentCountByTrackQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Enrollments
                .GetTable()
                .Where(e => e.TrackId == request.TrackId)
                .CountAsync();
        }
    }
}
