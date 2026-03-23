using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using MediatR;
using System.Collections;

namespace LMS___Mini_Version.Domain.CQRS.Interns.Query
{
    public record GetAllInternsQuery : IRequest<IEnumerable<InternDTO>>;
   


}
