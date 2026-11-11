using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.Queries;
using LMS___Mini_Version.features.shared;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.handler
{
    public class GetInternEnrollmentHistoryQueryHandler : IRequestHandler<GetInternEnrollmentHistoryQuery,RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>
    {
        private readonly IGeneralRepository<Intern> _repository;
        public GetInternEnrollmentHistoryQueryHandler(IGeneralRepository<Intern> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>> Handle(GetInternEnrollmentHistoryQuery request, CancellationToken cancellationToken)
        {
            if(request.InternId <= 0)
            {
                return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.Fail(400, new Dictionary<string, string[]> { { "InternId", new[] { "Intern Id must be greater than zero" }}});
            }
            var internExists = await _repository.GetAll().AnyAsync(i => i.Id == request.InternId, cancellationToken);

            if (!internExists)
            {
                return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.Fail(404,new Dictionary<string, string[]>{{ "Intern", new[] { "Intern not found" }}});
            }
            var enrollmentHistory = await _repository.GetAll()
                .Where(i => i.Id == request.InternId)
                .SelectMany(i => i.Enrollments)
                .OrderByDescending(e => e.EnrollmentDate)
                .Select(e => new GetInternEnrollmentHistoryDto
                {
                    EnrollmentId = e.Id,
                    TrackId = e.TrackId,
                    TrackName = e.Track.Name,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.Status
                })
                .ToListAsync(cancellationToken);
            return RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>.OK(enrollmentHistory);
        }
    }
}
