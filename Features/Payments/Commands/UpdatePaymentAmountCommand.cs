using MediatR;

namespace LMS___Mini_Version.Features.Payments.Commands;

public record UpdatePaymentAmountCommand(int EnrollmentId, decimal NewAmount) : IRequest;
