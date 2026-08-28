using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class RefundPaymentCommandHandler
    (IGeneralRepository<Payment> _paymentrepo,
        IUnitOfWork _unitOfWork): IRequestHandler<RefundPaymentCommand, Unit>
{

    public async Task<Unit> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentrepo.GetTable()
            .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentId, cancellationToken);

        if (payment != null)
        {
            payment.Status = PaymentStatus.Refunded;
        }

        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}