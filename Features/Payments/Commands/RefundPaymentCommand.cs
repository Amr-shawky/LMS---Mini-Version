using MediatR;

namespace LMS___Mini_Version.Features.Payments.Commands;

public record RefundPaymentCommand(int EnrollmentId) : IRequest;
