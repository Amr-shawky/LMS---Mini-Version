using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class GetTrackActiveEnrollmentCountQueryHandler : IRequestHandler<GetTrackActiveEnrollmentCountQuery, int>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetTrackActiveEnrollmentCountQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<int> Handle(GetTrackActiveEnrollmentCountQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepo.GetTable()
                .CountAsync(e => e.TrackId == request.TrackId && e.Status != EnrollmentStatus.Cancelled, cancellationToken);
        }
    }
}
