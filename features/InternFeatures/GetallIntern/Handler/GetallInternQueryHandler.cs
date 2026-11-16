using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.GetallIntern.DTOs;
using LMS___Mini_Version.features.InternFeatures.GetallIntern.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeatures.GetallIntern.Handler;

public class GetallInternQueryHandler(IGeneralRepository<Intern> repository) : IRequestHandler<GetallInternQuery, RequestResponse<IEnumerable<InternDTO>>>
{
    public async Task<RequestResponse<IEnumerable<InternDTO>>> Handle(GetallInternQuery request, CancellationToken cancellationToken)
    {
        var interns = await repository.GetAll()
            .Select(x=> new InternDTO { 
        
            FullName = x.FullName,
            Email = x.Email,
            Id = x.Id,
            Status = x.Status,
            TrackName = x.Track.Name,
        }).ToListAsync(cancellationToken);

        return  RequestResponse<IEnumerable<InternDTO>>.OK(interns,200);
    }
}
