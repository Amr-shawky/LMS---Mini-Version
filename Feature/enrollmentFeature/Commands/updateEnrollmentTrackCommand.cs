using exam_system.Features.Shared;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    /// <summary>
    /// Updates the TrackId of an existing Enrollment using SaveInclude,
    /// so only the TrackId column is written to the database.
    /// </summary>
    public record updateEnrollmentTrackCommand(int EnrollmentID, int TrackID) : IRequest<RequestResponse<bool>>;
}
