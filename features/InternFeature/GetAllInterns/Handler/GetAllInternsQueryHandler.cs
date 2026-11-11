using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Handler
{
    public class GetAllInternsQueryHandler
        : IRequestHandler<GetAllInternsQuery,
            RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(
            IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(
            GetAllInternsQuery request,
            CancellationToken cancellationToken)
        {
            var interns = await _internRepository.GetAll()
                .Select(i => new InternDto
                {
                    id = i.Id,
                    fullName = i.FullName,
                    email = i.Email,
                    status = i.Status,
                    trackName = i.Track.Name
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<InternDto>>.OK(interns);
        }
    }
}
