using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Queries;

public record GetTrackEnrollmentsQuery(int id) : IRequest<RequestResponse<IEnumerable<TrackEnrollmentDTO>>>;
