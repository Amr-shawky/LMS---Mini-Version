using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollmentStatus.Commands
{
    public record UpdateEnrollmentStatusCommand(int enrollmentId) : IRequest<RequestResponse>;
}
