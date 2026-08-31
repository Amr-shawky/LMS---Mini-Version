using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepositpry;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepositpry)
        {
            _internRepositpry = internRepositpry;
        }
        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns =await _internRepositpry.GetTable().Include(x => x.Track).ToListAsync(cancellationToken);
            return interns.Select(i => i.ToDto()).ToList();
        }
    }
}
