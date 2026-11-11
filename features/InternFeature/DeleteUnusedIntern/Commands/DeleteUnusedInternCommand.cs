using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.DeleteUnusedIntern.Commands
{
    public record DeleteUnusedInternCommand(int InternId) : IRequest<RequestResponse>;
    
}
