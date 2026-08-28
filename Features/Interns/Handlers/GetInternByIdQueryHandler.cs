using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
namespace LMS___Mini_Version.Features.Interns.Handlers;



public class GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepositry) : IRequestHandler<GetInternByIdQuery, InternDto?>
{
    
    public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
    {
        var intern = await internRepositry.GetByIdAsync(request.Id);

        return intern?.ToDto();

    }
}
