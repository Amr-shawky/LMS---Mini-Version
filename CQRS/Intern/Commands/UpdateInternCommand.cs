using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record UpdateInternCommand(int id, string FullName, string Email, int BirthYear, string Status, int TrackId, CancellationToken CancellationToken)
        :IRequest<ResultResponse<bool>>;
}
