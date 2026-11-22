using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.RegisterInternOrch.Orchestrator
{
    public record RegisterInternOrchestrator(string FullName, String Email, int BirthYear, int TrackId) : IRequest<RequestResponse<int>>;
   
}
