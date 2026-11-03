using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.EnrollIntern
{
    public class getActiveEnrollmentCountByTrackQueryHandler : IRequestHandler<getActiveEnrollmentCountByTrackQuery, int>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;

        public getActiveEnrollmentCountByTrackQueryHandler(IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepo = enrollmentrepo;
        }

        public async Task<int> Handle(getActiveEnrollmentCountByTrackQuery request, CancellationToken cancellationToken)
        {
            var activeEnrollmentTrackCount = await _enrollmentrepo.GetAll()
                .Where(e => e.TrackId == request.TrackID &&
                            e.Status == Domain.Enums.EnrollmentStatus.Active)
                .CountAsync(cancellationToken);

            return activeEnrollmentTrackCount;
        }
    }
}
