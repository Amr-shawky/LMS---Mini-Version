using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Commands
{
    public record CreateNewInternCommand : IRequest<bool>;
   
}
