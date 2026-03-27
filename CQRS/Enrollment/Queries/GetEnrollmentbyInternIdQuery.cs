using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record GetEnrollmentbyInternIdQuery(int internId) : IRequest<RequestResult<IEnumerable<EnrollmentDto>>>;
}
