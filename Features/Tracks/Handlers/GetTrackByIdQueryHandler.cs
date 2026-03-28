// ╔══════════════════════════════════════════════════════════════╗
// ║  🎯 ASSIGNMENT: Create GetTrackByIdQueryHandler here        ║
// ║                                                              ║
// ║  File: Features/Tracks/Handlers/GetTrackByIdQueryHandler.cs ║
// ║  See CQRS_Practice_Assignment.md → Task 1 for details      ║
// ╚══════════════════════════════════════════════════════════════╝

using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

public class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, TrackDto>
{
    private readonly IGeneralRepository<Track> _trackRepository;
    public GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepository)
    {
        _trackRepository = trackRepository;
    }
    public async Task<TrackDto> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.Id);
        return track?.ToDto();
    }
}