using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators
{
    public record TransferEnrollmentOrchestrator(int EnrollmentID, int newTrackID) : IRequest;

}
