using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            var enrollmentintern = _enrollmentRepository.Get().Where(e => e.InternId == request.InternId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    InternId = e.InternId,
                    TrackId = e.TrackId,
                    TrackName = e.Track.Name,
                    EnrollmentDate = e.EnrollmentDate,
                    Status = e.Status
                }).ToList();
            return Task.FromResult(enrollmentintern.AsEnumerable());
        }
    }
}
