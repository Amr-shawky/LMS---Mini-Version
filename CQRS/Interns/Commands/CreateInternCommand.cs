using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands
{
    public record CreateInternCommand(
        string FullName,
        string Email,
        int BirthYear,
        int TrackId,
        string Status
    ) : IRequest<CreateInternViewModel>;

}
