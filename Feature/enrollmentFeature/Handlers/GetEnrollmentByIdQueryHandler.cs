using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, RequestResponse<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepository;

        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepository = enrollmentrepo;
        }

        public async Task<RequestResponse<EnrollmentDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollmententity = await _enrollmentrepository.GetAll()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

            if (enrollmententity == null)
            {
                return RequestResponse<EnrollmentDto>.Fail($"Enrollment with ID {request.EnrollmentId} not found", 404);
            }

            var enrollmentdto = new EnrollmentDto
            {
                Id = enrollmententity.Id,
                InternId = enrollmententity.InternId,
                InternName = enrollmententity.Intern?.FullName ?? string.Empty,
                TrackId = enrollmententity.TrackId,
                TrackName = enrollmententity.Track?.Name ?? string.Empty,
                EnrollmentDate = enrollmententity.EnrollmentDate,
                Status = enrollmententity.Status
            };

            return RequestResponse<EnrollmentDto>.Ok(enrollmentdto);
        }
    }
}
