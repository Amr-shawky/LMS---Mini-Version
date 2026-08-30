using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record UpdateEnrollmentTrackCommand(int EnrollmentId, int NewTrackId) : IRequest;
