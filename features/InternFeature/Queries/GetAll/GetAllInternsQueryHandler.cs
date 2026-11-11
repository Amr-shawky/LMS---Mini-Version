using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Queries.GetAll;

public class GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
    : IRequestHandler<GetAllInternsQuery, RequestResponse<List<InternDto>>>
{
    public async Task<RequestResponse<List<InternDto>>> Handle(
        GetAllInternsQuery request, CancellationToken cancellationToken)
    {
        var interns = await internRepository.GetAll()
            .OrderBy(i => i.Id)
            .Select(i => new InternDto(
                i.Id,
                i.FullName,
                i.Email,
                i.Status,
                i.Track.Name))
            .ToListAsync(cancellationToken);

        return RequestResponse<List<InternDto>>.OK(interns);
    }
}