using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.CreateEnrollment.Commands
{
    public record CreateEnrollmentCommand(int InternId,int TrackId) : IRequest<RequestResponse<int>>;
}
