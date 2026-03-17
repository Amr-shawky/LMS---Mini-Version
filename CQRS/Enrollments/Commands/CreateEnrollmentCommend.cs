using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands
{
    public record CreateEnrollmentCommand(int internId, int trackId) : IRequest<EnrollInternViewModel>
    ;
}
