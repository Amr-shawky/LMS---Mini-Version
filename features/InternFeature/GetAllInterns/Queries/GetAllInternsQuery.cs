using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetAllInterns.Queries
{
    public record GetAllInternsQuery: IRequest<RequestResponse<IEnumerable<GetAllInternsDto>>>;
}
