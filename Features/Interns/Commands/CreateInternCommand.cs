using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record CreateInternCommand(InternDto dto)  : IRequest<CreateInternViewModel>;


}
