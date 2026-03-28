using LMS___Mini_Version.CQRS.RequestResult;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Queries
{
    public record IsInternEmailExistQuery(string email) : IRequest<RequestResult<bool>>;
}
