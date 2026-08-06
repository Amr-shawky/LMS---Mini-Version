using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Payments.Queries;

public record GetPaymentByEnrollmentIdQuery(int enrollmentId) : IQuery<PaymentDto?>;

public class GetPaymentByEnrollmentIdQueryHandler (IGeneralRepository<Payment> _paymentRepo)
    : IRequestHandler<GetPaymentByEnrollmentIdQuery, PaymentDto?>
{  
    public async Task<PaymentDto?> Handle(GetPaymentByEnrollmentIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepo.GetTable()
            .Where(p=> p.EnrollmentId == request.enrollmentId)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Method = p.Method,
                Status = p.Status
            }).FirstOrDefaultAsync(cancellationToken);

        return payment;
    }
}
