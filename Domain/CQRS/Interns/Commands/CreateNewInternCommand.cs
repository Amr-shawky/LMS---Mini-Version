using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Commands
{
    public record CreateNewInternCommand(
    string FullName,
    string Email,
    int BirthYear,
    string Status,
    int TrackId
    ) : IRequest<bool>;

}
