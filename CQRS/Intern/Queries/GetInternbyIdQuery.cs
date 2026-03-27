using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Intern.Queries
{
    public record GetInternbyIdQuery(int internId) : IRequest<RequestResult<InternDto>>;   
}
