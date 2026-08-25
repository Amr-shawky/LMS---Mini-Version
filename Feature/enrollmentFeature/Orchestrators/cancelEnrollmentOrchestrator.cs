using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators
{
    public record cancelEnrollmentOrchestrator (int enrollmentID) : IRequest;
    
}
