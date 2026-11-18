using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Commands
{
    public record CreateInternCommand(string FullName, string Email, int BirthYear, int TrackId): IRequest<RequestResponse<int>>;
    
}
