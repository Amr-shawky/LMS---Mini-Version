using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.GetAllInterns.Query
{
    public record GetAllInternsQuery : IRequest<RequestResponse<IEnumerable<InternDto>>>;
   
}
