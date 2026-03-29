using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands
{
    public record UpdateEnrollmentCommand(int id, string status) : IRequest<bool>;
}
