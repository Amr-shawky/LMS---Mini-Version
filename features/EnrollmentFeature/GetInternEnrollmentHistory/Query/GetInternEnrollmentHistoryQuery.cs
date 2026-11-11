using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Query
{
    public record GetInternEnrollmentHistoryQuery(int internId) : IRequest<RequestResponse<IEnumerable<EnrollmentHistoryResponseDto>>>;
}
