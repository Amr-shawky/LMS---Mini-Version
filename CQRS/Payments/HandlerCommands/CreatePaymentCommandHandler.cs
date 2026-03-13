using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.HandlerCommands;

public class CreatePaymentCommandHandler(IGeneralRepository<Payment> _paymentRepository) 
    : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    public Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment 
        {
            EnrollmentId = request.enrollmentId,
            Amount = request.amount,
            Method = request.method,
            PaymentDate = DateTime.UtcNow,
            Status = PaymentStatus.Pending,
        };

        _paymentRepository.Add(payment);

        return Task.FromResult(payment.ToDto());

    }
}
