using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }
        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            var enrollments=await _enrollmentRepo.GetAll().Where(en => en.InternId == request.InternId).Select(en => new EnrollmentDto
            {
                Id = en.Id,
                InternId = request.InternId,
                InternName = en.Intern.FullName,
                TrackId = en.TrackId,
                TrackName = en.Track.Name,
                EnrollmentDate = en.EnrollmentDate,
                Status = en.Status
            }).ToListAsync();
            return enrollments;
        }
    }
}
