using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record RefundPaymentCommand(int EnrollmentId) : IRequest<Unit>;