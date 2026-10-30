using MediatR;
using exam_system.Features.Shared;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Queries
{
    public record getActiveEnrollmentCountByTrackQuery(int TrackID) : IRequest<RequestResponse<int>>;

}
