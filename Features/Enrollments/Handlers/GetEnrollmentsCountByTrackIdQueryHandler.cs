using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsCountByTrackIdQueryHandler : IRequestHandler
                                       < GetEnrollmentsCountByTrackIdQuery, int>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetEnrollmentsCountByTrackIdQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<int> Handle(GetEnrollmentsCountByTrackIdQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepo.GetAll().Where(en => en.TrackId == request.TrackId).CountAsync();
        }
    }
}
