using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.RegisterIntern
{
    public record RegisterInternCommand(string fullName, string email, int birthYear, int trackId) : IRequest<RequestResponse<int>>;
    
 
}
