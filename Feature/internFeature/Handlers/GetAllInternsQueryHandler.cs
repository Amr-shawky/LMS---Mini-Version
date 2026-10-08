using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.internFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.internFeature.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await _internRepository.GetAll()
                .Select(i => new InternDto
                {
                    Id = i.Id,
                    FullName = i.FullName,
                    Email = i.Email,
                    BirthYear = i.BirthYear,
                    Status = i.Status,
                    TrackId = i.TrackId,
                    TrackName = i.Track != null ? i.Track.Name : string.Empty
                }).ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<InternDto>>.Ok(dtos);
        }
    }
}
