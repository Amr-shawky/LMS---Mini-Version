namespace LMS___Mini.CQRS.Interns.Commands.Requests
{
    public record DeleteInternCommand(int InternId) : IRequest<bool>;
}
