using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.AddTrack.Commands
{
    public record AddTrackCommand(string name , decimal Fees, int MaxCapacity) : IRequest<RequestResponse>;
    
}
