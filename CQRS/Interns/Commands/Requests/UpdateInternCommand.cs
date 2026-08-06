namespace LMS___Mini.CQRS.Interns.Commands.Requests
{
    public record UpdateInternCommand(int InternId, string Name, string Email, string phone, int TrackId) 
        : IRequest<int>;
}
