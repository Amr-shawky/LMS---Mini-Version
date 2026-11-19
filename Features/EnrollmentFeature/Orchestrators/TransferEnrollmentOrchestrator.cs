using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Orchestrators
{
    public record TransferEnrollmentOrchestrator(int enrollmentID, int newTrackID):IRequest<RequestResponse<bool>>;

}
