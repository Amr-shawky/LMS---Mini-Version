using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsByIdQueryHandler : IRequestHandler<GetEnrollmentsByIdQuery, EnrollmentDto>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepositpry;

        public GetEnrollmentsByIdQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepositpry = enrollmentRepository;
        }
        public async Task<EnrollmentDto> Handle(GetEnrollmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepositpry.GetTable()
                 .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (enrollment is null)
                throw new KeyNotFoundException($"EnrollmentId {request.Id} Not Found");
            return enrollment.ToDto();

        }
    }
}
