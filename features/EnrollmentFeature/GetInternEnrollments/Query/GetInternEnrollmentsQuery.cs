using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Query
{
    public record GetInternEnrollmentsQuery(int InternId)
        : IRequest<RequestResponse<IEnumerable<InternEnrollmentHistoryDto>>>;
}
