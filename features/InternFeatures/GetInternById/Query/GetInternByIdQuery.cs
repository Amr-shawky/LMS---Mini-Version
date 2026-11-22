using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.GetInternById.Query
{
    public record GetInternByIdQuery(int id) : IRequest<RequestResponse<InternDto>>;
    
}
