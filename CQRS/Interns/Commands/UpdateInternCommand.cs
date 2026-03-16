using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands
{
    public record UpdateInternCommand(int id,string FullName, string Email, int BirthYear, string Status, int TrackId) : IRequest<bool>
    {
    }
}
