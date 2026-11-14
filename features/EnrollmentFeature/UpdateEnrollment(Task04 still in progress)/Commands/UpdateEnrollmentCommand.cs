using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollment.Commands
{
    public class UpdateEnrollmentCommand(int enrollmentId) : IRequest<RequestResponse>
    {
    }
}
