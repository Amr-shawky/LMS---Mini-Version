using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands
{
    public record DeleteEnrollmentCommand(int id) : IRequest<bool>;
}
