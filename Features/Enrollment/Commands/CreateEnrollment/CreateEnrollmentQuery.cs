using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.Commands.CreateEnrollment
{
    public record CreateEnrollmentQuery(CreateEnrollmentDto dto) :IRequest<EnrollmentDto>
    {
    }
}
