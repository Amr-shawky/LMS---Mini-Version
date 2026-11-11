using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentsFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentsFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentsFeature.Handler
{
    public class GetTrackEnrollmentsQueryHandler : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentDTO>>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetTrackEnrollmentsQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<TrackEnrollmentDTO>>> Handle(GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _repository
           .GetAll()
           .Where(e => e.TrackId == request.trackId)
           .Select(e => new TrackEnrollmentDTO
           {
               TrackId = e.TrackId,
               EnrollementId = e.Id,

               InternFullName = e.Intern.FullName,

               EnrollmentDate = e.EnrollmentDate,

               enrollementStatus = e.Status,

               PaymentAmount = e.Payment == null
                   ? null
                   : e.Payment.Amount,

               PaymentStatus = e.Payment == null
                   ? null
                   : e.Payment.Status
           })
           .ToListAsync(cancellationToken);
            if (enrollments is null || !enrollments.Any())
            {
                return RequestResponse<IEnumerable<TrackEnrollmentDTO>>.OK([], 200);
            }
            return RequestResponse<IEnumerable<TrackEnrollmentDTO>>.OK(enrollments, 200);
        }
    }
}
