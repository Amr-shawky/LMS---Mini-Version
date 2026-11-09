using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.InternEnrollment
{
    public record GatInternEnrollmentQuery(int InternId) : IRequest<RequestResponse<IEnumerable<InternEnrollmentDto>>>;


}
