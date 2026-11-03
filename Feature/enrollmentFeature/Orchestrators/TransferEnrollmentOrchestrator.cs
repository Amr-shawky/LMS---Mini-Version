using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators
{
    public record TransferEnrollmentOrchestrator(int PreviousEnrollmentID, int newTrackID) : IRequest<RequestResponse<int>>;

}
