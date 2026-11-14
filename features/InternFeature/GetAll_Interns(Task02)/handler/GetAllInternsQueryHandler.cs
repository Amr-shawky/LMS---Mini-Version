using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAll_Interns.Dtos;
using LMS___Mini_Version.features.InternFeature.GetAll_Interns.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.GetAll_Interns.handler
{
    public class GetAllInternsQueryHandler: IRequestHandler<GetAllInternsQuery, RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }
        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(
            GetAllInternsQuery request,
            CancellationToken cancellationToken)
        {
            var internDtos = await _internRepository.GetAll()
                .Select(x => new InternDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    Status = x.Status,
                    TrackName = x.Track.Name
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<InternDto>>.OK(internDtos);
        }
    }
}
