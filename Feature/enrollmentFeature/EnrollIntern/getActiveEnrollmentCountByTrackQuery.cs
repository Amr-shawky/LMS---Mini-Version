using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.EnrollIntern
{
    public record getActiveEnrollmentCountByTrackQuery(int TrackID) : IRequest<int>;
}
