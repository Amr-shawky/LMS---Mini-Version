using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackAvailability.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.TrackAvailability.Queries;

public record TrackAvailabilityQuery(int Id) : IRequest<RequestResponse<TrackAvailabilityDTo>>;

