using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record CancelEnrollmentOrchestratorCommand(int EnrollmentId) : IRequest<Unit>;