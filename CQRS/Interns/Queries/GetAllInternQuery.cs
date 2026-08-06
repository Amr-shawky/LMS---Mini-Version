using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Queries;

public record GetAllInternQuery : IQuery<IEnumerable<InternDto>>;

public class GetAllInternQueryHandler (IGeneralRepository<Domain.Entities.Intern> _internRepo)
    : IRequestHandler<GetAllInternQuery, IEnumerable<InternDto>>
{ 
    public async Task<IEnumerable<InternDto>> Handle(GetAllInternQuery request, CancellationToken cancellationToken)
    {
        var interns = await _internRepo.GetTable()
            .Select(i => new InternDto
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                //BirthYear = i.BirthYear, /// can send it if needed but i think it's not necessary
                Status = i.Status,
                TrackName = i.Track.Name
            }).ToListAsync();

        return interns;
    }
}
