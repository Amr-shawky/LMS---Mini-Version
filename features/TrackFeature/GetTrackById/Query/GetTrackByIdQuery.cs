using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Query
{
    public record GetTrackByIdQuery(int TrackId) : IRequest<RequestResponse<TrackDto>>;
  
}
