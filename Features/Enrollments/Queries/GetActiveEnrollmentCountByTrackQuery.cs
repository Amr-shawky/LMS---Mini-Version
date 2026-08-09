using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetActiveEnrollmentCountByTrackQuery(int TrackId) : IRequest<int>;

    public class GetActiveEnrollmentCountByTrackQueryHandler : IRequestHandler<GetActiveEnrollmentCountByTrackQuery, int>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetActiveEnrollmentCountByTrackQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<int> Handle(GetActiveEnrollmentCountByTrackQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository
                .GetTable()
                .CountAsync(e => e.TrackId == request.TrackId && e.Status != EnrollmentStatus.Cancelled, cancellationToken);
        }
    }
}
