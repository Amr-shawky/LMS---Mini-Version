using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Commands
{
    public record UpdateInternCommand(int id,string FullName, string Email, int BirthYear, InternStatus Status) : IRequest<bool>;

}
