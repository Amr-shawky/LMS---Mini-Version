using MediatR;

namespace LMS___Mini_Version.Domain.CQRS.Enrollments.Command
{
    public record UpdateEnrollmentTrackCommand(int enrollmentId, int newTrackId):IRequest<bool>;
   
}
