using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query
{
    public record GetAllEnrollmentQuery(int InternId) : IRequest<RequestResponse<List<GetAllEnrollmentsDTO>>>;
}
