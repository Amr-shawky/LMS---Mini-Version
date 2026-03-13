using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Intern.Queries;

public record GetInternByIdQuery(int id) : IQuery<InternDto?>;


public class GetInternByIdQueryHandler (IGeneralRepository<Domain.Entities.Intern> _internRepo)
    : IRequestHandler<GetInternByIdQuery, InternDto?>
{   
    public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
    {
        var intern = await _internRepo.GetTable()
            .Where(i => i.Id == request.id)
            .Select(i => new InternDto
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                BirthYear = i.BirthYear, /// can send it if needed but i think it's not necessary
                Status = i.Status,
                TrackId = i.TrackId,
                TrackName = i.Track.Name
            }).FirstOrDefaultAsync(cancellationToken);
        return intern;
    }
}
