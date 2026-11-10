using LMS___Mini_Version.features.InternFeature.BrowseInterns.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.BrowseInterns.Query
{
    public record BrowseInternsQuery() : IRequest<RequestResponse<IEnumerable<InternResponseDto>>>;
}
