using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Queries.Requests
{
    public record GetEnrollmentByIdQuery(int EnrollmentId) : IRequest<EnrollmentDto>;
}
