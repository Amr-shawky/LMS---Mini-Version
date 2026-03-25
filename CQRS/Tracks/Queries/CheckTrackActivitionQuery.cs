using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record CheckTrackActivitionQuery(int trackId):IRequest<RequestResult<bool>>;
   
}
