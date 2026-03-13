using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Payments.Queries;

public record GetAllPaymentQuery : IQuery<IEnumerable<PaymentDto>>;

public class GetAllPaymentQueryHandler (IGeneralRepository<Payment> _paymentRepo)
    : IRequestHandler<GetAllPaymentQuery, IEnumerable<PaymentDto>>
{ 
    public async Task<IEnumerable<PaymentDto>> Handle(GetAllPaymentQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepo.GetTable().Select(p => new PaymentDto 
        {
            Id = p.Id,
            EnrollmentId = p.EnrollmentId,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            Method = p.Method,
            Status = p.Status
        }).ToListAsync();

        return payments;
    }
}
