using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.internFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.internFeature.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, RequestResponse<PaginatedResult<InternDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<PaginatedResult<InternDto>>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var query = _internRepository.GetAll();

            var dtos = await query
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
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

            var totalCount = await query.CountAsync(cancellationToken);

            var paginatedResult = PaginatedResult<InternDto>.Create(
                dtos,
                totalCount,
                request.PageIndex,
                request.PageSize
            );

            return RequestResponse<PaginatedResult<InternDto>>.Ok(paginatedResult);
        }
    }
}
