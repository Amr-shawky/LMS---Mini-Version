using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.DeleteIntern
{
    public record DeleteInternCommand(int internId) : IRequest<RequestResponse>;
}
