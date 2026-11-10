using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries
{
    public record GetInternByIdQuery(int internId) : IRequest<RequestResponse<GetInternByIdDTO>>;
    
}
