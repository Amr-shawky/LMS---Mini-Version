using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands
{
    public record CreateEnrollmentCommand(int internId, int trackId) : IRequest<EnrollmentResultDto>;
}
