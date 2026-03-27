using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands
{
    public record CreateEnrollmentCommand(int internId, int trackId) : IRequest<bool>;
}
