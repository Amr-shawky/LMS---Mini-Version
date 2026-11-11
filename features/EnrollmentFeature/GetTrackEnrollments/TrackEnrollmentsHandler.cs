using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments
{
    public class TrackEnrollmentsHandler : IRequestHandler<TrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentsDto>>>
    {
        readonly private IGeneralRepository<Enrollment> _enrollmentRepository;
        public TrackEnrollmentsHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<IEnumerable<TrackEnrollmentsDto>>> Handle(TrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<TrackEnrollmentsDto> trackEnrollmentsDtos = _enrollmentRepository.GetAll().Where(e => e.TrackId == request.trackId).Select(e => new TrackEnrollmentsDto
            {
                EnrollmentId = e.Id,
                InternFullName = e.Intern.FullName,
                EnrollmentStatus = e.Status,
                EnrollmentDate = e.EnrollmentDate,
                PaymentAmount = e.Payment.Amount,
                PaymentStatus = e.Payment.Status
            });
            if(!trackEnrollmentsDtos.Any())
                return RequestResponse<IEnumerable<TrackEnrollmentsDto>>.Fail(404, ErrorCode.TrackNotFound);
            return RequestResponse<IEnumerable<TrackEnrollmentsDto>>.OK(trackEnrollmentsDtos);
        }
    }
}
