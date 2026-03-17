using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Queries
{
    public record GetAllInternsQuery(CancellationToken CancellationToken, int page = 1) :IRequest<ResultResponse<IEnumerable<InternDto>>>;
   
}
