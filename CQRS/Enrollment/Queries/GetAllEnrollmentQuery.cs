using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record GetAllEnrollmentQuery() : IRequest<RequestResult<IEnumerable<EnrollmentDto>>>;
}
