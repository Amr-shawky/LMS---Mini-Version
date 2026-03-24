using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Queries
{
    public record CheckCapacityQuery(int trackId):IRequest<bool>;
   
}
