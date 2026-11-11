using LMS___Mini_Version.features.EnrollmentsFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentsFeature.Queries
{
    public record GetInternEnrollmentHistoryQuery(int InternId) : IRequest<RequestResponse<IEnumerable<EnrollmentHistoryDto>>>;

}
