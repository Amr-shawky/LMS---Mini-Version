using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.Command
{
    public record RegisterInternCommand(string FullName , string Email , int BirthYear , int TrackId) : IRequest<RequestResponse<int>>;
}
