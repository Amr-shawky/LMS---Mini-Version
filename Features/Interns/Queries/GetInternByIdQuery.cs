using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetInternByIdQuery(int Id) : IRequest<InternDto>;

    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto>
    {
        private readonly IGeneralRepository<Intern> _internrepository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internrepository)
        {
            _internrepository = internrepository;
        }

        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            //projection hit the navigation propety to get the track name
            var internDto = await _internrepository.GetTable()
                .Select(x => new InternDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    BirthYear = x.BirthYear,
                    Email = x.Email,
                    Status = x.Status,
                    TrackId = x.TrackId,
                    TrackName = x.Track.Name
                })
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return internDto;
        }
    }
}
