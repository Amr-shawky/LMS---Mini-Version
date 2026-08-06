namespace LMS___Mini.CQRS.Interns.Queries.Requests
{
    public record GetAllByTrackIdQuery(int trackId):IRequest<IEnumerable<InternDto>>;
}
