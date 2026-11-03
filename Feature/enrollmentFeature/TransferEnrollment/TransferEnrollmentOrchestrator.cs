using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public record TransferEnrollmentOrchestrator(int EnrollmentID, int newTrackID) : IRequest<RequestResponse>;
}
