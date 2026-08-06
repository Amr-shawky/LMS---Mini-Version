using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Commands;

public record CreatePaymentCommand(int enrollmentId, decimal amount, PaymentMethod method) : IRequest<PaymentDto>;


