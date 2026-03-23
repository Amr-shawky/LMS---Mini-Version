using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Command
{
    public record UpdateStatusEnrollmentCommand(int enrollmentId, EnrollmentStatus newStatus) : IRequest<bool>;
   
}
