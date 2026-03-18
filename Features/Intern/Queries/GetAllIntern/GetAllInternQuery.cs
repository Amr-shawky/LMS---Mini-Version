using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Queries.GetAllIntern
{
    public record GetAllInternQuery():IRequest<IEnumerable<InternDto>>{}
}
