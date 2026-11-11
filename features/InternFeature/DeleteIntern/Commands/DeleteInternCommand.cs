using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern.Commands
{
    public record DeleteInternCommand(int InternId)
        : IRequest<RequestResponse>;
}
