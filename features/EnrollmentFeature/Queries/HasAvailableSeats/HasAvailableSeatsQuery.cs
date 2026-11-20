using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.HasAvailableSeats;

public record HasAvailableSeatsQuery(int TrackId, int MaxCapacity) : IRequest<bool>;