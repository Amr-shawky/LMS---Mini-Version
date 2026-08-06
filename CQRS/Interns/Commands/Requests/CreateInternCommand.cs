namespace LMS___Mini.CQRS.Interns.Commands.Requests
{
    public record CreateInternCommand(string Name, int TrackId, string Email, string Phone) : IRequest<int>;
    
}
