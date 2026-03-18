using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Commands.CreateIntern
{
    public record  CreateInternCommand(InternDto dto) :IRequest<InternDto>
    {
    }
}
