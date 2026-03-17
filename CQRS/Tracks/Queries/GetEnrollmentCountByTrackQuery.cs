using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{

    public record GetEnrollmentCountByTrackQuery(int TrackId) : IRequest<int>;
}
