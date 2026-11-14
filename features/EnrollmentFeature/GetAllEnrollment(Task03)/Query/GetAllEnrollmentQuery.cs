using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query
{
    public class GetAllEnrollmentQuery:IRequest<RequestResponse<IEnumerable<EnrollmentDto>>>
    {
        public int InternId { get; set; }
        public GetAllEnrollmentQuery(int internid)
        {
            InternId = internid;
        }
    }
}
