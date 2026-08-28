using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class AdjustPaymentAmountCommandHandler
    (IGeneralRepository<Payment> _paymentrepo):
    IRequestHandler<AdjustPaymentAmountCommand, Unit>
{

    public async Task<Unit> Handle(AdjustPaymentAmountCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentrepo.GetTable()
            .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentId, cancellationToken);

        if (payment != null)
        {
            payment.Amount = request.NewAmount;
        }
        
        return Unit.Value;
    }
}