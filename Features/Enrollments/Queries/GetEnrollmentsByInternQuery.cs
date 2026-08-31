using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetEnrollmentsByInternQuery(int internId) : IRequest<IEnumerable<EnrollmentDto>>;
    
}
