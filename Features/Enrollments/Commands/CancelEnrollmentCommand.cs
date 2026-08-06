using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record CancelEnrollmentCommand(int Id) : IRequest<bool>;

