using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.ApproveIntern.Commands
{
    public record ApproveInternCommand(int InternId): IRequest<RequestResponse>;
}
