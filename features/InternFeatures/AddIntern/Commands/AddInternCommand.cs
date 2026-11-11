using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.AddIntern.Commands
{
    public record AddInternCommand(string FullName, string Email, int BirthYear,int TrackId) : IRequest<RequestResponse>
    {
    }
}
