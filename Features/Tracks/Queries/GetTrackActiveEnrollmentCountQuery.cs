using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Queries;

public record GetTrackActiveEnrollmentCountQuery(int TrackId) : IRequest<int>;
