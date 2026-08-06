using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Requests
{
    public record CancelEnrollmentCommand(int EnrollmentId) : IRequest<EnrollmentResultDto>;
}
