using LMS___Mini_Version.CQRS.RequestResult;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Track.Queries
{
    public record GetAllTrackQuery() : IRequest<RequestResult<IEnumerable<TrackDto>>>;
}
