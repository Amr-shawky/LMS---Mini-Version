using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Query
{
    public record GetTrackEnrollmentsQuery(int TrackId) : IRequest<RequestResponse<IEnumerable<TrackEnrollmentsDto>>>; 

}
