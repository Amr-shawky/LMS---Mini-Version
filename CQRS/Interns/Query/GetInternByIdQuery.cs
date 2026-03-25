using LMS___Mini_Version.Infrastructure.DTO_S.InternDTo;
using MediatR;

namespace LMS___Mini_Version.CQRS.Interns.Query
{
    public record GetInternByIdQuery(int Id):IRequest<InternSummaryDTO?>;
   
}
