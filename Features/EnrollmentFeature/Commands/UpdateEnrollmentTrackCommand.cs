using Azure.Core;
using LMS___Mini_Version.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Commands
{
    public record UpdateEnrollmentTrackCommand(int EnrollmentID , int TrackID): IRequest<RequestResponse<bool>>;

}
