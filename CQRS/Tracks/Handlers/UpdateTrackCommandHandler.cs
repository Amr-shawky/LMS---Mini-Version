using LMS___Mini_Version.CQRS.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.CQRS.Tracks.Handlers
{
    public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand, bool>
    {
        public Task<bool> Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
