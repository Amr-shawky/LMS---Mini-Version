using LMS___Mini_Version.features.InternFeature.GetInternById.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.GetInternById.Queries
{
    public record GetInternByIdQuery(int Id) : IRequest<RequestResponse<GetInternByIdDto>>;
}
