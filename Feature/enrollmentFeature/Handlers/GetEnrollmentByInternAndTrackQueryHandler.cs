using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class GetEnrollmentByInternAndTrackQueryHandler:IRequestHandler<GetEnrollmentByInternAndTrackQuery, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetEnrollmentByInternAndTrackQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<RequestResponse<bool>> Handle(GetEnrollmentByInternAndTrackQuery request, CancellationToken cancellationToken)
        {
            var enrollmentExists = await _enrollmentRepo.GetAll()
                .AnyAsync(e => e.InternId == request.internId && e.TrackId == request.trackId, cancellationToken);
            return RequestResponse<bool>.Ok(enrollmentExists);
        }
    }
}
