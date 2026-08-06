namespace LMS___Mini.CQRS.Interns.Queries.Requests
{
    public record GetAllInternsQuery() : IRequest<IEnumerable<InternDto>>;
}
