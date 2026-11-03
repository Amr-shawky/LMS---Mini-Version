using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record UpdateEnrollmentTrackCommand(int EnrollmentId, int TrackId) : IRequest<Unit>;
}
