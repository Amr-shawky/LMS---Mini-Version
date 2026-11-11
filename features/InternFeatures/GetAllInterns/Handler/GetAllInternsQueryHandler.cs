using Azure.Core;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace LMS___Mini_Version.features.InternFeatures.GetAllInterns.Handler
{
    public class GetAllInternsQueryHandler(IGeneralRepository<Intern> repository) : IRequestHandler<GetAllInternsQuery, RequestResponse<IEnumerable<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> repository = repository;

        public async Task<RequestResponse<IEnumerable<InternDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var internsDto = await repository.GetAll().Select(x => new InternDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName = x.Track.Name
            }).ToListAsync();

            return RequestResponse<IEnumerable<InternDto>>.OK(internsDto);
        }
    }
}
