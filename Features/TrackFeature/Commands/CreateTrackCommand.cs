using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.TrackFeature.Commands
{
    public record CreateTrackCommand(string name , decimal Fees , int MaxCapacity , bool? IsActive = true) : IRequest<RequestResponse>;

}
