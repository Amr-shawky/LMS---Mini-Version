using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public sealed class RefundPaymentCommandHandler : IRequestHandler<refundPaymentCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;
        private readonly IMediator _mediator;

        public RefundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepository, IMediator mediator)
        {
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<RequestResponse<bool>> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID, cancellationToken);

            if (payment is null || payment.Status != PaymentStatus.Completed)
            {
                return RequestResponse<bool>.Ok(false, "No eligible payment was found.");
            }

            var previousStatus = payment.Status;
            payment.Status = PaymentStatus.Refunded;
            _paymentRepository.SaveInclude(payment, nameof(Payment.Status));

            var history = await _mediator.Send(
                new CreateRefundHistoryCommand(payment.Id, payment.EnrollmentId, previousStatus), cancellationToken);

            return history.Success
                ? RequestResponse<bool>.Ok(true, "Payment refunded successfully.")
                : RequestResponse<bool>.Fail(history.Message, history.StatusCode, history.Errors);
        }
    }
}