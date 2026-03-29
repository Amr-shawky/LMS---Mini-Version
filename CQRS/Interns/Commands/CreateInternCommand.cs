using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Commands
{
    public record CreateInternCommand(
        string fullName,
        string email,
        int birthYear,
        string status,
        int trackId
    ) : IRequest<InternDto>;
}
