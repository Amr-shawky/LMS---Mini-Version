using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.Commands
{
    public record CreateInternCommand(string FullName,string Email,int BirthYear,int TrackId) : IRequest<RequestResponse<int>>;
}
