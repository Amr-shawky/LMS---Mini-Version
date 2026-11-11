using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.AddIntern.Commands
{
    public record AddInternCommand(string fullName , string email , int BirthYear , int TrackId) : IRequest<RequestResponse<int>>
    {
    }
}
