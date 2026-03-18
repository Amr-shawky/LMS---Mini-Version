using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.UpdateIntern
{
    public record UpdateInternCommand(int id, InternDto dto) :IRequest<bool>
    {
    }
}
