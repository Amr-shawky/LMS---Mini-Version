using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries
{
    /// <summary>
    /// Query to retrieve a single track by its Id.
    /// Returns TrackDto (the handler maps Entity → DTO).
    /// 
    /// NOTE: This query is also used internally by Orchestrators
    /// (EnrollInternOrchestrator, TransferEnrollmentOrchestrator).
    /// </summary>
    public record GetTrackByIdQuery(int Id) : IRequest<TrackDto>;
}