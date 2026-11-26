using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.handler
{
    public class GetTrackEnrollmentsQueryHandler
        : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<TrackEnrollmentDto>>>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetTrackEnrollmentsQueryHandler(
            IGeneralRepository<Track> trackRepository,
            IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _trackRepository = trackRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<IEnumerable<TrackEnrollmentDto>>> Handle(
            GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var trackExists = await _trackRepository.GetAll()
                .AnyAsync(t => t.Id == request.TrackId, cancellationToken);

            if (!trackExists)
                return RequestResponse<IEnumerable<TrackEnrollmentDto>>.Fail(StatusCodes.Status404NotFound,
                    nameof(request.TrackId), $"Track with id {request.TrackId} was not found.");

            // Filtered by Enrollment.TrackId (not Intern.TrackId).
            // Intern is a required join; Payment is optional, so EF Core produces a LEFT JOIN
            // and the payment columns come back as null when no payment exists.
            var enrollments = await _enrollmentRepository.GetAll()
                .Where(e => e.TrackId == request.TrackId)
                .OrderByDescending(e => e.EnrollmentDate)
                .ThenByDescending(e => e.Id)
                .Select(e => new TrackEnrollmentDto
                {
                    EnrollmentId = e.Id,
                    InternFullName = e.Intern.FullName,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.Status,
                    PaymentAmount = e.Payment == null ? (decimal?)null : e.Payment.Amount,
                    PaymentStatus = e.Payment == null ? (PaymentStatus?)null : e.Payment.Status
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<TrackEnrollmentDto>>.OK(enrollments);
        }
    }
}
