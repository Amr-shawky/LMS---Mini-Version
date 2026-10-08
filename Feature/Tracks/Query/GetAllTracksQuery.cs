using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Feature.Tracks.Query
{
    public record GetAllTracksQuery : IRequest<RequestResponse<IEnumerable<TrackDto>>>;
}
