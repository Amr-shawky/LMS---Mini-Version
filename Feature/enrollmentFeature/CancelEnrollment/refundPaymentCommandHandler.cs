using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
    public class refundPaymentCommandHandler : IRequestHandler<refundPaymentCommand, RequestResponse<RefundResult>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public refundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<RefundResult>> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID, cancellationToken);

            if (payment == null)
            {
                return RequestResponse<RefundResult>.Ok(new RefundResult
                {
                    IsEligible = false
                }, "No payment record exists for this enrollment.");
            }

            if (payment.Status != PaymentStatus.Completed)
            {
                return RequestResponse<RefundResult>.Ok(new RefundResult
                {
                    IsEligible = false,
                    PaymentId = payment.Id,
                    PreviousStatus = payment.Status
                }, $"Payment status is '{payment.Status}', not eligible for refund.");
            }

            var previousStatus = payment.Status;
            payment.Status = PaymentStatus.Refunded;
            _paymentRepository.Update(payment);
            await _paymentRepository.SaveChangesAsync();

            return RequestResponse<RefundResult>.Ok(new RefundResult
            {
                IsEligible = true,
                PaymentId = payment.Id,
                PreviousStatus = previousStatus
            }, "Payment refunded successfully.");
        }
    }
}

