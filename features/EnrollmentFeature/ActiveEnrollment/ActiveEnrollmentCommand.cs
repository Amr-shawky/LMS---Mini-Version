using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.ActiveEnrollment
{
    public record ActiveEnrollmentCommand(int internId, int trackId) :IRequest<RequestResponse<int>>;

}
