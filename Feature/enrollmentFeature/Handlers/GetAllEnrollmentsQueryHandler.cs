using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, RequestResponse<PaginatedResult<EnrollmentDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetAllEnrollmentsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse<PaginatedResult<EnrollmentDto>>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var skip = request.PageSize * (request.PageIndex - 1);

            var query = _enrollmentRepo.GetAll();

            var dtos = await query
                .OrderByDescending(e=>e.EnrollmentDate)
                .ThenBy(e => e.Id)
                .Skip(skip)
                .Take(request.PageSize)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    InternName = e.Intern != null ? e.Intern.FullName : string.Empty,
                    TrackId = e.TrackId,
                    TrackName = e.Track != null ? e.Track.Name : string.Empty,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                }).ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            var paginatedResult = PaginatedResult<EnrollmentDto>.Create(
                dtos,
                totalCount,
                request.PageIndex,
                request.PageSize
            );

            return RequestResponse<PaginatedResult<EnrollmentDto>>.Ok(paginatedResult, "Enrollments retrieved successfully.");
        }
    }
}
