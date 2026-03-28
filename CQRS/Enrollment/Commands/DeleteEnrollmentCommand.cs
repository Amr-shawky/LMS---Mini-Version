using LMS___Mini_Version.CQRS.RequestResult;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands
{
    public record DeleteEnrollmentCommand(int id) : IRequest<RequestResult<bool>>;
}
