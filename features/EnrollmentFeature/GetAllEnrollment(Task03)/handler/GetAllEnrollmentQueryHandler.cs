using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.shared;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.handler
{
    public class GetInternEnrollmentHistoryQueryHandler :IRequestHandler<GetAllEnrollmentQuery,
        RequestResponse<IEnumerable<EnrollmentDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetInternEnrollmentHistoryQueryHandler(
            IGeneralRepository<Enrollment> enrollmentRepository,
            IGeneralRepository<Intern> internRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _internRepository = internRepository;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentDto>>> Handle(GetAllEnrollmentQuery request, CancellationToken cancellationToken)
        {
            // Check if the intern exists
            var intern = await _internRepository.GetByIdAsync(request.InternId);
            if (intern == null)
            {
                return RequestResponse<IEnumerable<EnrollmentDto>>.Fail(404,
                    new Dictionary<string, string[]>
                    {
                        { "InternId", new[] { "Intern not found." } }
                    });
            }
            // Get the enrollment history for the intern
            var enrollments = await _enrollmentRepository.GetAll()
                .Where(e => e.InternId == request.InternId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.Id,
                    TrackId = e.TrackId,
                    TrackName = e.Track.Name,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.Status
                })
                .ToListAsync(cancellationToken);
            // Return the enrollment history
            return RequestResponse<IEnumerable<EnrollmentDto>>.OK(enrollments);

        }
    }
}
