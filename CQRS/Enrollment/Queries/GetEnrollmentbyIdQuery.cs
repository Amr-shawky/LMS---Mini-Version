using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Queries
{
    public record GetEnrollmentbyIdQuery(int id) : IRequest<RequestResult<EnrollmentDto>>;
}
