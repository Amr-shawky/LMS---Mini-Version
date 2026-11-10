using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.InternEnrollment
{
    public class InternEnrollmentHandler : IRequestHandler<GatInternEnrollmentQuery, RequestResponse<IEnumerable<InternEnrollmentDto>>>
    {
        readonly private IGeneralRepository<Intern> _internRepository;
        readonly private IGeneralRepository<Enrollment> _enrollmentRepository;
        public InternEnrollmentHandler(IGeneralRepository<Intern> internRepository, IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _internRepository = internRepository;
            _enrollmentRepository = enrollmentRepository;

        }
        public async Task<RequestResponse<IEnumerable<InternEnrollmentDto>>> Handle(GatInternEnrollmentQuery request, CancellationToken cancellationToken)
        {
            Intern intern = _internRepository.GetAll().FirstOrDefault(i => i.Id == request.InternId);
            if(intern == null)
            {
                return RequestResponse<IEnumerable<InternEnrollmentDto>>.Fail(404,ErrorCode.InternNotFound);
            }

            IEnumerable<InternEnrollmentDto> enrollments = _enrollmentRepository.GetAll().Where(e => e.Id == request.InternId).Select(e => new InternEnrollmentDto
            {
                EnrollmentId = e.Id,
                TrackName = e.Track.Name,
                EnrollmentStatus = e.Status,
                EnrollmentDate = e.EnrollmentDate,
            }).OrderBy(e => e.EnrollmentDate);
            return RequestResponse<IEnumerable<InternEnrollmentDto>>.OK(enrollments);
            
        }
    }
}
