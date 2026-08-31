using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetInternByIdQueryHandler : IRequestHandler<GetInternByIdQuery, InternDto?>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }
        public async Task<InternDto?> Handle(GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern= await _internRepository.GetTable()
                .Include(x => x.Track)
                .FirstOrDefaultAsync(i => i.Id == request.id, cancellationToken);
            return intern == null ? null : intern.ToDto();
        }
    }
}
