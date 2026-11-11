using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands
{
    public record CompleteEnrollmentCommand(int EnrollmentId) : IRequest<RequestResponse>;
 
}
