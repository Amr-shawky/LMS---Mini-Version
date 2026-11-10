using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries
{
    public record GetInternEnrollmentHistoryQuery(int internId):IRequest<RequestResponse<IEnumerable<GetInternEnrollmentHistoryDto>>>
    {
    }
}
