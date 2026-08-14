using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Query
{
    public record GetAllTrackQuery : IRequest<IEnumerable<TrackDto>>;


}
