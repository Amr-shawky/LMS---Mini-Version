using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.handler
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, RequestResponse<IEnumerable<InternListItemDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<IEnumerable<InternListItemDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            // All statuses are included; TrackName is joined in SQL through the navigation property.
            var interns = await _internRepository.GetAll()
                .OrderBy(i => i.Id)
                .Select(i => new InternListItemDto
                {
                    Id = i.Id,
                    FullName = i.FullName,
                    Email = i.Email,
                    Status = i.Status,
                    TrackName = i.Track.Name
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<InternListItemDto>>.OK(interns);
        }
    }
}
