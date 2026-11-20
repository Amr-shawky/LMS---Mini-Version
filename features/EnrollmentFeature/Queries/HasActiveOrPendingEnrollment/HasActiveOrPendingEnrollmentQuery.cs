using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Queries.HasActiveOrPendingEnrollment;

public record HasActiveOrPendingEnrollmentQuery(
    int InternId, 
    int TrackId) : IRequest<bool>;
