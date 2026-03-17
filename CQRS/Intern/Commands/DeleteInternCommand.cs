using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record DeleteInternCommand(int id,CancellationToken CancellationToken):IRequest<ResultResponse<bool>>;
   
}
