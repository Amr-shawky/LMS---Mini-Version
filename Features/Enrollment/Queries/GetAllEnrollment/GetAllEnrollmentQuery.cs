using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetAllEnrollment
{
    public record GetAllEnrollmentQuery():IRequest<IEnumerable<EnrollmentDto>>
    {
    }
}
