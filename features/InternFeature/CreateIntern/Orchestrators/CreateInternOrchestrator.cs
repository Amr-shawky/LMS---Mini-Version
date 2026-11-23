using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.CreateIntern.Orchestrators
{
    public record CreateInternOrchestrator(string FullName, string Email, int BirthYear, int TrackId) : IRequest<RequestResponse<int>>; 
}
