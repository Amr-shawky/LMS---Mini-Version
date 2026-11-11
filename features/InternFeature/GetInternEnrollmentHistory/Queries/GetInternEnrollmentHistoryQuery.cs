using LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.Queries
{
    public record GetInternEnrollmentHistoryQuery(int InternId) : IRequest<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>;
}
