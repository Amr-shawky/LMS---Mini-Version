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

        private readonly IGeneralRepository<Intern> _intern;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> interns)
        {
            _intern = interns;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {

            return await _intern.GetTableNoTracking()
                .Include(i => i.Track)
                .Select(i => i.ToDto())
                .ToListAsync(cancellationToken);

             

        }

    }



}
