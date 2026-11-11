using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollmentsHistory.GetEnrollmentsHistoryQuery
{
    public record GetAllEnrollmentsQuery(int InternId) : IRequest<RequestResponse<IEnumerable<EnrollmentDto>>>;
    

}
