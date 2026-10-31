using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    /// <summary>
    /// Persists a RefundHistory record for a completed refund.
    ///
    /// Single repository (R8): only IGeneralRepository of RefundHistory is injected.
    /// The PaymentId is passed in the command by the caller, which already knows it
    /// from the refund context. This avoids needing a second repository here.
    ///
    /// IMPORTANT: This handler runs inside the same UOF transaction as refundPaymentCommand.
    /// If this handler throws, the caller rolls back the whole transaction so the payment
    /// status is not left as Refunded without a history record (R24, R25).
    /// </summary>
    public class CreateRefundHistoryCommandHandler : IRequestHandler<CreateRefundHistoryCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<RefundHistory> _historyRepository;

        public CreateRefundHistoryCommandHandler(IGeneralRepository<RefundHistory> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<RequestResponse<bool>> Handle(CreateRefundHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = new RefundHistory
            {
                PaymentId = request.PaymentId,
                EnrollmentId = request.EnrollmentId,
                PreviousStatus = request.PreviousStatus.ToString(),
                NewStatus = request.NewStatus.ToString(),
                RefundedAt = DateTime.UtcNow
            };

            _historyRepository.Add(history);
            await _historyRepository.SaveChangesAsync();

            return RequestResponse<bool>.Ok(true, "Refund history recorded.");
        }
    }
}