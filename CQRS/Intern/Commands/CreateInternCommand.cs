using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record CreateInternCommand(string FullName, string Email, int BirthYear, InternStatus Status) :IRequest<RequestResult<bool>>;
}
