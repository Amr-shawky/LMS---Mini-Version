using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers;

public sealed class refundPaymentCommandHandler(IGeneralRepository<Payment> repository)
    : IRequestHandler<refundPaymentCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(refundPaymentCommand request, CancellationToken ct)
    {
        // Refund eligibility depends on the ORIGINAL payment, not destination track fees.
        var payment = await repository.GetAll().AsTracking()
            .SingleOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID, ct);
        if (payment == null || payment.Status != PaymentStatus.Completed)
            return RequestResponse<int>.Ok(0, "No completed original payment to refund");

        payment.Status = PaymentStatus.Refunded;
        repository.SaveInclude(payment, nameof(Payment.Status));
        await repository.SaveChangesAsync(); // within caller's UOF transaction
        return RequestResponse<int>.Ok(payment.Id);
    }
}
