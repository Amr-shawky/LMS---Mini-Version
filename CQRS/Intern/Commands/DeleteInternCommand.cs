using LMS___Mini_Version.CQRS.RequestResult;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record DeleteInternCommand(int Id) : IRequest<RequestResult<bool>>;

}
