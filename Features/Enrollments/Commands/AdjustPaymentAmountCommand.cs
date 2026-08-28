using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands;

public record AdjustPaymentAmountCommand(int EnrollmentId, decimal NewAmount) : IRequest<Unit>;