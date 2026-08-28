using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Queries;

public record GetActiveEnrollmentsCountQuery(int TrackId) : IRequest<int>;