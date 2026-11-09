using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.DTOs;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Queries;

public record AvailabilityQuery(int TrackId) : IRequest<RequestResponse<TrackAvailabilityDto>>;
