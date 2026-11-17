using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetAllInternsQuery : IRequest<IEnumerable<GetAllinternsDTO>>;

    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<GetAllinternsDTO>>
    {
        private readonly IGeneralRepository<Intern> _internRepo;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepo)
        {
            _internRepo = internRepo;
        }

        public async Task<IEnumerable<GetAllinternsDTO>> Handle(GetAllInternsQuery query,CancellationToken cancellationToken)
        {
            var interns = _internRepo.GetTable().Include(i => i.Track).ToList();
            var allinterns = interns.Select(x => new GetAllinternsDTO
            {
                id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName = x.Track.Name
            });
            
            return allinterns;
        }
    }
}
