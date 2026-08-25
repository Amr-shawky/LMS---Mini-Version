using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record updateEnrollmentTrackCommand(int EnrollmentID, int TrackID) : IRequest;
    
}
