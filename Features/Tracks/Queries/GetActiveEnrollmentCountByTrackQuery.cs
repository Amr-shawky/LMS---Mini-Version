using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    public record GetActiveEnrollmentCountByTrackQuery (int TrackId) : IRequest<int>;
    
}
