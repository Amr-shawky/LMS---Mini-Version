using MediatR;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record getActiveEnrollmentCountByTrackQuery(int TrackID) : IRequest<int>;

}
