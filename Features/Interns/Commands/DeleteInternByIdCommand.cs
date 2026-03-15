using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record DeleteInternByIdCommand(int id) : IRequest<bool>;
    
    
    
}
