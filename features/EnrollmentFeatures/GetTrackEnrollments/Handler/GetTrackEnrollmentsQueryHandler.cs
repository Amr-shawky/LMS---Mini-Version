using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Handler
{
    public class GetTrackEnrollmentsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo) : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentsDto>>>
    {
        private readonly IGeneralRepository<Enrollment> enrollmentRepo = enrollmentRepo;

        public async Task<RequestResponse<IEnumerable<TrackEnrollmentsDto>>> Handle(GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var trackEnrollmentDto = await enrollmentRepo.GetAll().Where(x => x.TrackId == request.TrackId)
                .Select(x => new TrackEnrollmentsDto
                {
                    EnrollmentId = x.Id,
                    InternFullName = x.Intern.FullName,
                    EnrollmentDate = x.EnrollmentDate,
                    EnrollmentStatus = x.Status,
                    PaymentAmount = x.Payment == null ? (decimal?)null : x.Payment.Amount,
                    PaymentStatus = x.Payment ==null ? (PaymentStatus?)null : x.Payment.Status
                }).ToListAsync();

            return RequestResponse<IEnumerable<TrackEnrollmentsDto>>.OK(trackEnrollmentDto);
        }
    }
}
