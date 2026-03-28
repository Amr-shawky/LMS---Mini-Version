using LMS___Mini_Version.CQRS.RequestResult;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands
{
    public record UpdateEnrollmentCommand(int id, int? internId, int? trackId) : IRequest<RequestResult<bool>>;
}
