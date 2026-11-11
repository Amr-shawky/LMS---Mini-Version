using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.handler
{
    public class GetTrackEnrollmentsQueryHandler : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>>
    {
        private readonly IGeneralRepository<Track> _repository;
        public GetTrackEnrollmentsQueryHandler(IGeneralRepository<Track> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>> Handle(GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.TrackId <= 0)
            {
                return RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>.Fail(400, new Dictionary<string, string[]> { { "TrackId", new[] { "TrackId must be greater than 0." } } });
            }
            var trackData = await _repository.GetAll()
                .Where(t => t.Id == request.TrackId)
                .Select(t => new
                {
                    Enrollments = t.Enrollments.Select(e => new GetTrackEnrollmentsDto
                    {
                        EnrollmentId = e.Id,
                        InternFullName = e.Intern.FullName,
                        EnrollmentDate = e.EnrollmentDate,
                        EnrollmentStatus = e.Status,

                        PaymentAmount = e.Payment == null? null: e.Payment.Amount,

                        PaymentStatus = e.Payment == null? null: (PaymentStatus?)e.Payment.Status
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (trackData == null)
            {
                return RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>.Fail(404, new Dictionary<string, string[]> { { "Track", new[] { "Track not found." } } });
            }
            
            return RequestResponse<IEnumerable<GetTrackEnrollmentsDto>>.OK(trackData.Enrollments);
        }
    }
}
