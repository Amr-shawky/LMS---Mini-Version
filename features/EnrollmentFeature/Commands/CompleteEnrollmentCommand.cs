using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Commands
{
    public record CompleteEnrollmentCommand(int id): IRequest<RequestResponse>
    {
    }
}
