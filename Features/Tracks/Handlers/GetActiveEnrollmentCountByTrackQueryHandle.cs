using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class GetActiveEnrollmentCountByTrackQueryHandle : IRequestHandler<GetActiveEnrollmentCountByTrackQuery, int>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetActiveEnrollmentCountByTrackQueryHandle(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public Task<int> Handle(GetActiveEnrollmentCountByTrackQuery request, CancellationToken cancellationToken)
        {
           var count=  _enrollmentRepository.GetTable()
                .Where(e => e.TrackId == request.TrackId && e.Status == EnrollmentStatus.Active)
                .CountAsync(cancellationToken);
            return count;
        }
    }
}
