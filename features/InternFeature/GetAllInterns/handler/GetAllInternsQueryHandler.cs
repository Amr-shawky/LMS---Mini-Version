using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.handler
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, RequestResponse<IEnumerable<GetAllInternsDto>>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public GetAllInternsQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<GetAllInternsDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = _repository.GetAll();
            var internDtos = await interns.Select(intern => new GetAllInternsDto
            {
                Id = intern.Id,
                FullName = intern.FullName,
                Email = intern.Email,
                TrackName = intern.Track.Name ?? string.Empty,
                Status = intern.Status.ToString()
            }).ToListAsync(cancellationToken);
            return RequestResponse<IEnumerable<GetAllInternsDto>>.OK(internDtos);
        }
    }
}
