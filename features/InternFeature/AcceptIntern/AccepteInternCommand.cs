using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.AcceptIntern
{
    public record AccepteInternCommand(int internId) : IRequest<RequestResponse<int>>;

}
