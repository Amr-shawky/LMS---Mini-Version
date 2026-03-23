using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Command
{
    public record UpdateEnrollmentTrackCommand(int enrollmentId, int newTrackId):IRequest<bool>;
   
}
