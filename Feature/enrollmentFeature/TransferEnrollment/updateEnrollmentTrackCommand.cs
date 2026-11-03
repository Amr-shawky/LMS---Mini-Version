using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public record updateEnrollmentTrackCommand(int EnrollmentID, int TrackID) : IRequest;
}
