using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace LMS___Mini_Version.Feature.Tracks.Handlers
{
    public class GetAllTrackQueryHandler : IRequestHandler<GetAllTrackQuery, RequestResponse<PaginatedResult<TrackDto>>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        public GetAllTrackQueryHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }
        public async Task<RequestResponse<PaginatedResult<TrackDto>>> Handle(GetAllTrackQuery request, CancellationToken cancellationToken)
        {
            var query = _trackRepository.GetAll();

            var tracks = await query
                .Skip((request.PageSize * (request.PageIndex-1)))
                .Take(request.PageSize)
                .Select(t => new TrackDto
            {
                Id = t.Id,
                Name = t.Name,
                Fees = t.Fees,
                IsActive = t.IsActive,
                MaxCapacity = t.MaxCapacity
            }).ToListAsync();

            var totalCount = await query.CountAsync(cancellationToken);

            PaginatedResult<TrackDto> paginatedResult = PaginatedResult<TrackDto>.Create(
                tracks,
                totalCount,
                request.PageIndex,
                request.PageSize
                );

            var response = RequestResponse<PaginatedResult<TrackDto>>.Ok(paginatedResult);

            return response;
        }
    }
}
