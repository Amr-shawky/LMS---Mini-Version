using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeatures.AddIntern.Orchestrator
{
    public record AddInternOrchestrator(string FullName,string Email,int BirthYear,int TrackId) : IRequest<RequestResponse<int>>;

}
