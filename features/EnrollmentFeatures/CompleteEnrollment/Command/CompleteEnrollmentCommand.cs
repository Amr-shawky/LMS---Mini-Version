using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.CompleteEnrollment.Command
{
    public record CompleteEnrollmentCommand(int EnrollmentId) : IRequest<RequestResponse>;
   
}
