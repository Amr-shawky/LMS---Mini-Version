using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Queries.GetEnrollmentByInternId
{
    public record GetEnrollmentByInternIdQuery(int internId):IRequest<IEnumerable<EnrollmentDto>>
    {
    }
}
