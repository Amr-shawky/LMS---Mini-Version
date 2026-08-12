using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepo;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepo)
        {
            _internRepo = internRepo;
        }

        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _internRepo.GetAll().Select(intern=> new InternDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email=intern.Email,
                BirthYear=intern.BirthYear,
                Status=intern.Status,
                TrackId =intern.TrackId,
                TrackName = intern.Track.Name,
            }).ToListAsync();
            return interns;
        }
    }
}
