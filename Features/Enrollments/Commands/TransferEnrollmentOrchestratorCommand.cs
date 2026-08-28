using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record TransferEnrollmentOrchestratorCommand(int EnrollmentId, int NewTrackId) : IRequest<bool>;