using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetAllInternsQuery : IRequest<IEnumerable<InternDto>>;


    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _internrepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internrepository)
        {
            _internrepository = internrepository;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _internrepository.GetAllAsync();
            var internsDto = interns.Select(x => new InternDto
            {
                FullName = x.FullName,
                BirthYear = x.BirthYear,
                Email = x.Email,
                Status = x.Status,
                TrackId = x.TrackId,
                TrackName = x.Track?.Name
            });

            return internsDto;
        }
    }
}
