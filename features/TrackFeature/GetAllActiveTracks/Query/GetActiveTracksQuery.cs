using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.DTOs;
using MediatR;
using System.Collections.Generic;

namespace LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Query
{
    public record GetActiveTracksQuery : IRequest<RequestResponse<IEnumerable<ActiveTracksDTO>>>;
    
    
}
