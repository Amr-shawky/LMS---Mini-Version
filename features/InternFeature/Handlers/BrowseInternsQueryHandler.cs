using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
{
    public class BrowseInternsQueryHandler : IRequestHandler<BrowseInternsQuery, RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _repository;

        public BrowseInternsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(BrowseInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _repository.GetAll().Select(i => new InternDto()
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                Status = i.Status,
                TrackName = i.Track.Name
            }).ToListAsync();

            return RequestResponse<IEnumerable<InternDto>>.OK(interns, 200);
        }
    }
}
