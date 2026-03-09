namespace LMS___Mini_Version.Features.Intern.Queries.GetAllInterns
{
    using LMS___Mini_Version.DTOs;
    using MediatR;

    public record GetAllInternQuery():IRequest<IEnumerable<InternDto>>
    {

    }
}
