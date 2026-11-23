using LMS___Mini_Version.features.EnrollementFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollementFeature.Orchestrator
{
    public record GetEnrollementsWithTrackIdOrchestrator(int trackId) : IRequest<RequestResponse<IEnumerable<EnrollementByTrackIdDTO>>>;
    
    
}
