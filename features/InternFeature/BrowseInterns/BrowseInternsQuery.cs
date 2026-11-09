using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.BrowseInterns
{
    public record BrowseInternsQuery : IRequest<RequestResponse<IEnumerable<BrowseInternsDto>>>;
   
}
