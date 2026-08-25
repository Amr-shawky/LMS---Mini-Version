using LMS___Mini_Version.Domain.Enums;
using MediatR;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record updateEnrollmentStatusCommand(int enrollmentId , EnrollmentStatus
Status ) :IRequest;
    
}
