using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands
{
    public record UpdateInternCommand(
        int id,
        string fullName,
        string email,
        int birthYear,
        string status,
        int trackId
    ) : IRequest<bool>;
}
