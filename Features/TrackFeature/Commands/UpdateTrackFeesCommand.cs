using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Commands
{
    public record UpdateTrackFeesCommand(decimal fees,int ID) : IRequest<RequestResponse>;
}
