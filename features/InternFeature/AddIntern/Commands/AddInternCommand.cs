using LMS___Mini_Version.features.InternFeature.AddIntern.DTOS;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.Commands
{
    public record AddInternCommand(CreateInternDTO InternDto) : IRequest<RequestResponse<int>>;
}
