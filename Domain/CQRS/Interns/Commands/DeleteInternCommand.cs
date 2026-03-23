using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Commands
{
    public record DeleteInternCommand(int Id) : IRequest<bool>;
  
}
