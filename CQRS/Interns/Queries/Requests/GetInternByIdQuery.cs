namespace LMS___Mini.CQRS.Interns.Queries.Requests
{
    public record GetInternByIdQuery(int id):IRequest<InternDto?>;
}
