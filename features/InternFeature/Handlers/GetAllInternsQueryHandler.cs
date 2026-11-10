using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.Handlers
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
            var internsDto = await _repository.GetAll()
                .Select(x => new GetAllInternsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName = x.Track.Name
            }).ToListAsync();

            
            return RequestResponse<IEnumerable<GetAllInternsDto>>.OK(internsDto);
            

     
        }
    }
}
