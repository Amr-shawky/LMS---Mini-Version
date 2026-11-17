using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.DTOs;
using LMS___Mini_Version.Features.Interns.DTOs;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetInternEnrollmentHistoryQuery(int Internid) : IRequest<IEnumerable<ViewInternEnrollmentHistoryDTO>>;

    public class GetInternEnrollmentHistoryQueryHandler : IRequestHandler<GetInternEnrollmentHistoryQuery, IEnumerable<ViewInternEnrollmentHistoryDTO>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IGeneralRepository<Intern> _internRepo;
        public GetInternEnrollmentHistoryQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo, IGeneralRepository<Intern> internrepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _internRepo = internrepo;
        }

        public IGeneralRepository<Intern> Internrepo { get; }

        public async Task<IEnumerable<ViewInternEnrollmentHistoryDTO>> Handle(GetInternEnrollmentHistoryQuery query, CancellationToken cancellationToken)
        {
            var intern = _internRepo.GetById(query.Internid);
            if (intern == null)
                return null;

            var InternEnrollments = await _enrollmentRepo.GetTable().Where(e => e.InternId == query.Internid).Select(x=>new ViewInternEnrollmentHistoryDTO
            {
                EnrollmentId = x.Id,
                TrackId = x.TrackId,
                TrackName = x.Track.Name,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.Status,
            }).OrderBy(x=>x.EnrollmentDate).ToListAsync(cancellationToken);

            return InternEnrollments;
        }
    }
}

