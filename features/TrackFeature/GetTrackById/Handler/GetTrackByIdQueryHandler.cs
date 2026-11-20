using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Handler;

public class GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepository)
    : IRequestHandler<GetTrackByIdQuery, Track?>
{
    public async Task<Track?> Handle(GetTrackByIdQuery request, CancellationToken ct)
        => await trackRepository.GetByIdAsync(request.TrackId);
}