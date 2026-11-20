using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;

public record GetTrackByIdQuery(int TrackId) : IRequest<Track?>;