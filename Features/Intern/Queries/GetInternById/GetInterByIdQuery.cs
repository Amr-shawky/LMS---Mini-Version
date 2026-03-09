using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Intern.Queries.GetInternById
{
    public record GetInterByIdQuery (int Id) : IRequest<InternDto?>;
    
    }

