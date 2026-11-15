using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.InternEnrollment
{
    public class InternEnrollmentHandler : IRequestHandler<GatInternEnrollmentQuery, RequestResponse<IEnumerable<InternEnrollmentDto>>>
    {
        readonly private IGeneralRepository<Enrollment> _enrollmentRepository;
        public InternEnrollmentHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;

        }
        public async Task<RequestResponse<IEnumerable<InternEnrollmentDto>>> Handle(GatInternEnrollmentQuery request, CancellationToken cancellationToken)
        {

            IEnumerable<InternEnrollmentDto> enrollments = await _enrollmentRepository.GetAll().Where(e => e.InternId == request.InternId).Select(e => new InternEnrollmentDto
            {
                EnrollmentId = e.Id,
                TrackId = e.TrackId,
                TrackName = e.Track.Name,
                EnrollmentStatus = e.Status,
                EnrollmentDate = e.EnrollmentDate,
            }).OrderByDescending(e => e.EnrollmentDate).ToListAsync(cancellationToken);

            if(!enrollments.Any())
            {
                return RequestResponse<IEnumerable<InternEnrollmentDto>>.Fail(404,ErrorCode.EnrollmentNotFound);
            }
            return RequestResponse<IEnumerable<InternEnrollmentDto>>.OK(enrollments);
            
        }
    }
}
