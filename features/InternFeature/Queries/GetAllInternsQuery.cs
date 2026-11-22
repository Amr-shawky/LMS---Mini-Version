using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Queries
{
    public record GetAllInternsQuery(int PageIndex, int PageSize): IRequest<RequestResponse<IEnumerable<GetAllInternsDto>>>
    {
    }
}
