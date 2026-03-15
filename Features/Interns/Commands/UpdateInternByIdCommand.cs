using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record UpdateInternByIdCommand(int Id, UpdateInternViewModel Vm) : IRequest<Unit>;
    
   
}
