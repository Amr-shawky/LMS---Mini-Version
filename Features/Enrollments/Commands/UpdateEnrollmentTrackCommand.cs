using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record UpdateEnrollmentTrackCommand (int EnrollmentID , int NewTrackID):IRequest;
    
}
