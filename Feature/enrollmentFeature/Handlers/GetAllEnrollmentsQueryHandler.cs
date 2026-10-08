using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, RequestResponse<IEnumerable<EnrollmentDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetAllEnrollmentsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentDto>>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await _enrollmentRepository.GetAll()
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

            return RequestResponse<IEnumerable<EnrollmentDto>>.Ok(dtos);
        }
    }
}
