using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.CheckInternEnrollmentExists.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.CheckInternEnrollmentExists.handler
{
    public class CheckInternEnrollmentExistsQueryHandler : IRequestHandler<CheckInternEnrollmentExistsQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _repository;
        public CheckInternEnrollmentExistsQueryHandler(IGeneralRepository<Enrollment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResponse<bool>> Handle(CheckInternEnrollmentExistsQuery request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();

            if (request.InternId <= 0)
            {
                errors.Add("InternId", new[] { "Intern ID must be greater than zero." });
            }
            if (request.TrackId <= 0)
            {
                errors.Add("TrackId", new[] { "Track ID must be greater than zero." });
            }
            if (errors.Count > 0)
            {
                return RequestResponse<bool>.Fail(400, errors);
            }
            var exists = await _repository.GetAll()
                .AnyAsync(e => e.InternId == request.InternId && e.TrackId == request.TrackId && (e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active), cancellationToken);
            return RequestResponse<bool>.OK(exists);
        }
    }
}
