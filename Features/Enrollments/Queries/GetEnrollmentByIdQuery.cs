using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetEnrollmentByIdQuery(int Id) : IRequest<EnrollmentDto>;



    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, EnrollmentDto>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetEnrollmentByIdQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        // (Include) not a correct way to get a child fields , it get unnecessary fields that i donot use it __ (select *)
        public async Task<EnrollmentDto> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository
                .GetTable()
                .Include(e => e.Intern)
                .Include(e => e.Track)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            return enrollment.ToDto();
        }
    }

}
