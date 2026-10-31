using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    /// <summary>
    /// Handles refundPaymentCommand.
    ///
    /// Business rule (R13): only a Payment with Status == Completed is eligible.
    /// Returns the PaymentId when a refund was applied, 0 otherwise (no-op).
    ///
    /// Idempotency (R26): if the payment is already Refunded, returns 0 without writing anything.
    ///
    /// Single repository (R8): only IGeneralRepository of Payment is injected.
    /// </summary>
    public class refundPaymentCommandHandler : IRequestHandler<refundPaymentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public refundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<int>> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID, cancellationToken);

            if (payment == null)
                return RequestResponse<int>.Ok(0, "No payment found for this enrollment; nothing to refund.");

            if (payment.Status != PaymentStatus.Completed)
                return RequestResponse<int>.Ok(0,
                    $"Payment status is '{payment.Status}'; only Completed payments can be refunded.");

            // Update the status on the object, then use SaveInclude to write only the Status column.
            payment.Status = PaymentStatus.Refunded;
            _paymentRepository.SaveInclude(payment, nameof(Payment.Status));
            await _paymentRepository.SaveChangesAsync();

            // Return the PaymentId so the caller can pass it to CreateRefundHistoryCommand.
            return RequestResponse<int>.Ok(payment.Id, "Payment refunded successfully.");
        }
    }
}