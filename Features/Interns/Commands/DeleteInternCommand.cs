using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record DeleteInternCommand(int id) : IRequest<bool>;
}
