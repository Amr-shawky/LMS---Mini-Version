using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.internFeature.Queries
{
    public record GetAllInternsQuery : IRequest<RequestResponse<IEnumerable<InternDto>>>;
}
