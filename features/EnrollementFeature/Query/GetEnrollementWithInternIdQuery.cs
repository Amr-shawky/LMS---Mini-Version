using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Query
{
    public record GetEnrollementWithInternIdQuery(int internId) :IRequest<RequestResponse<IEnumerable<EnrollementByInternIdDTO>>>;
    
}
