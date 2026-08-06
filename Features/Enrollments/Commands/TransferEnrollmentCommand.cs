using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record TransferEnrollmentCommand(
    int EnrollmentId,
    int NewTrackId
) : IRequest<bool>;
