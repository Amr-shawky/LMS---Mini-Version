using LMS___Mini_Version.ViewModels.Track;
using MediatR;

namespace LMS___Mini_Version.Features.Payments.Commands
{
    public record CreateTrackCommand(string Name , decimal Fees , bool IsActive , int MaxCapacity)
        : IRequest<TrackSummaryViewModel>;
    
}
