using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Orchestrator
{
    public record GetEnrollementWithInternIdOrchestrator(int InternId) : IRequest<RequestResponse<IEnumerable<EnrollementByInternIdDTO>>>;
    
    
}
