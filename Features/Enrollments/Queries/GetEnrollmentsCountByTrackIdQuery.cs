using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public class GetEnrollmentsCountByTrackIdQuery:IRequest<int>
    {
        public int TrackId { get; set; }
    }
}
