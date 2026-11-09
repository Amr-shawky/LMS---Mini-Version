using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackEnrollments.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.TrackEnrollments.Query;

public record GetTrackEnrollmentsQuery(int TrackId) : IRequest<RequestResponse<List<TrackEnrollmentDto>>>;
