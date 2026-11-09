using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.InternEnrollment
{
    public class InternEnrollmentHandler : IRequestHandler<GatInternEnrollmentQuery, RequestResponse<IEnumerable<InternEnrollmentDto>>>
    {
        readonly private IGeneralRepository<Enrollment> _generalRepository;
        public InternEnrollmentHandler(IGeneralRepository<Enrollment> generalRepository)
        {
            _generalRepository = generalRepository;
        }
        public async Task<RequestResponse<IEnumerable<InternEnrollmentDto>>> Handle(GatInternEnrollmentQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<InternEnrollmentDto> enrollments = _generalRepository.GetAll().Where(e => e.InternId == request.InternId).Select(e => new InternEnrollmentDto
            {
                EnrollmentId = e.Id,
                TrackName = e.Track.Name,
                EnrollmentStatus = e.Status,
                EnrollmentDate = e.EnrollmentDate,
            }
            );
            return RequestResponse<IEnumerable<InternEnrollmentDto>>.OK(enrollments);
            
        }
    }
}
