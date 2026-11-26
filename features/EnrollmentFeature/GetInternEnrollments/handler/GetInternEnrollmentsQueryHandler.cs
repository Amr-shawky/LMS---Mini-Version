using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Query;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.handler
{
    public class GetInternEnrollmentsQueryHandler
        : IRequestHandler<GetInternEnrollmentsQuery, RequestResponse<IEnumerable<InternEnrollmentHistoryDto>>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetInternEnrollmentsQueryHandler(
            IGeneralRepository<Intern> internRepository,
            IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _internRepository = internRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<IEnumerable<InternEnrollmentHistoryDto>>> Handle(
            GetInternEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var internExists = await _internRepository.GetAll()
                .AnyAsync(i => i.Id == request.InternId, cancellationToken);

            if (!internExists)
                return RequestResponse<IEnumerable<InternEnrollmentHistoryDto>>.Fail(StatusCodes.Status404NotFound,
                    nameof(request.InternId), $"Intern with id {request.InternId} was not found.");

            // Every status is included, newest first.
            // TrackName comes from each enrollment's own track (e.Track), not the intern's current track.
            var history = await _enrollmentRepository.GetAll()
                .Where(e => e.InternId == request.InternId)
                .OrderByDescending(e => e.EnrollmentDate)
                .ThenByDescending(e => e.Id)
                .Select(e => new InternEnrollmentHistoryDto
                {
                    EnrollmentId = e.Id,
                    TrackId = e.TrackId,
                    TrackName = e.Track.Name,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.Status
                })
                .ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<InternEnrollmentHistoryDto>>.OK(history);
        }
    }
}
