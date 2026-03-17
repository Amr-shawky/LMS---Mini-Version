using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record CreateInternCommand(string fullName, string Email, int BirthYear, string Status, int TrackId,CancellationToken CancellationToken)
        :IRequest<ResultResponse<String>>;
   
}
