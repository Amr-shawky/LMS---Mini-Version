using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.DeleteIntern
{
    public record DeleteInternCommand(int id) :IRequest<bool>
    {
    }
}
