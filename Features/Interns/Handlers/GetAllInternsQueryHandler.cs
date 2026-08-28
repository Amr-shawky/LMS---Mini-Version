using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using MediatR;
namespace LMS___Mini_Version.Features.Interns.Handlers;



public class GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository) : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>   
{



     async Task<IEnumerable<InternDto>> IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>.Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
    {
        var interns = await internRepository.GetAllAsync();

        return interns.Select(x => x.ToDto());
    }
}
