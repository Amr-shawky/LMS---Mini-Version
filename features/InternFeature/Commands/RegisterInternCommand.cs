using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Commands
{
    public record RegisterInternCommand(
        string FullName,
        string Email,
        int BirthYear,
        int TrackId
        ) : IRequest<RequestResponse<RegisterInternDto>>;
     
}
