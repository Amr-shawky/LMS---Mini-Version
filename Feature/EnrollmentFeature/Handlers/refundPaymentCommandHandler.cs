using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.EnrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Handlers
{
    public class refundPaymentCommandHandler : IRequestHandler<refundPaymentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;
        private readonly IMediator _mediator;

        public refundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepo, IMediator mediator)
        {
            _paymentRepo = paymentRepo;
            _mediator = mediator;
        }
        public async Task<RequestResponse<int>> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            //1.Get the PaymentID from the EnrollmentID :
            var payment = await _paymentRepo.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID, cancellationToken);

            if (payment == null)
                return RequestResponse<int>.Ok(0,"No payment found for this enrollment. Nothing to refund.");

            if (payment.Status != PaymentStatus.Completed)
                return RequestResponse<int>.Ok(0,$"Payment status is {payment.Status}. Not eligible for refund.");

            //2. Update the payment status to Refunded by SaveInclude :
            var update = new Payment { Id = payment.Id, Status = PaymentStatus.Refunded };
            _paymentRepo.SaveInclude(update, nameof(Payment.Status));

            return RequestResponse<int>.Ok(payment.Id, "Payment refunded successfully.");
        }
    }
}
