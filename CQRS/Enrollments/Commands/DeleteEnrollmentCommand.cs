using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands
{
    public record DeleteEnrollmentCommand(int id) : IRequest<bool>;
    
}
