using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Queries
{
    public record GetByIdInternsQuery(int Id,CancellationToken CancellationToken) : IRequest<ResultResponse<InternDto>>;

}
