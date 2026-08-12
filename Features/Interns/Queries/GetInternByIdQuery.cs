using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetInternByIdQuery:IRequest<InternDto?>
    {
        public int Id { get; set; }
    }
}
