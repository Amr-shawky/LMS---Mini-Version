using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands
{
    public record UpdateEnrollmentCommand(int Id, int internId, int trackId) : IRequest<EnrollmentViewModel>;
    
}
