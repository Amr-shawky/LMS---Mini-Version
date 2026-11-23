using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Orchestrator
{
    public record AssignInternToEnrollementOrchreatrator(string fullName, string Email, int birthYear, int trackId) : IRequest<RequestResponse<CreateInternDTO>>;
    
    
}
