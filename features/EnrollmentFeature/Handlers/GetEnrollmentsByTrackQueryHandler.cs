using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Handlers
{
    public class GetEnrollmentsByTrackQueryHandler : IRequestHandler<GetEnrollmentsByTrackQuery, RequestResponse<IEnumerable<TrackEnrollmentsDto>>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;

        public GetEnrollmentsByTrackQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<IEnumerable<TrackEnrollmentsDto>>> Handle(GetEnrollmentsByTrackQuery request, CancellationToken cancellationToken)
        {
            var enrollments = await _repository.GetAll()
                .Where(e => e.TrackId == request.TrackId)
            .Select(e => new TrackEnrollmentsDto
            {
                InternId = e.InternId,
                EnrollmentStatus = e.Status.ToString(),
            }).ToListAsync(cancellationToken);

            

            return RequestResponse<IEnumerable<TrackEnrollmentsDto>>.OK(enrollments);
        }
    }
}
