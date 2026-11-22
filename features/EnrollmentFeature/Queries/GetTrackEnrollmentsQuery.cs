using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries
{
    public record GetTrackEnrollmentsQuery(int TrackId): IRequest<RequestResponse<IEnumerable<EnrollmentsForTrackDto>>>
    {
    }

    public class GetTrackEnrollmentsQueryHandler : IRequestHandler<GetTrackEnrollmentsQuery, RequestResponse<IEnumerable<EnrollmentsForTrackDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetTrackEnrollmentsQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<IEnumerable<EnrollmentsForTrackDto>>> Handle(GetTrackEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _repository.GetAll()
                .Where(e => e.TrackId == request.TrackId)
            .Select(e => new EnrollmentsForTrackDto
            {
                EnrollmentId = e.Id,
                InternFullName = e.Intern.FullName,
                EnrollmentDate = DateOnly.FromDateTime(e.EnrollmentDate),
                EnrollmentStatus = e.Status.ToString(),
                PaymentAmount = e.Payment == null ? null : e.Payment.Amount,
                PaymentStatus = e.Payment == null ? "" : e.Payment.Status.ToString(),

            }).ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<EnrollmentsForTrackDto>>.OK(enrollments);


        }
    }
}
