using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record UpdateTrackCommand(int id , string name , decimal fees , 
        bool IsActive , int MaxCapacity) : IRequest<Unit>;
    



}
