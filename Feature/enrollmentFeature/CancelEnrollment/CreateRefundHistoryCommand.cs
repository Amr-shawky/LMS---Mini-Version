using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
    public record CreateRefundHistoryCommand(
        int PaymentId,
        int EnrollmentId,
        PaymentStatus PreviousStatus,
        PaymentStatus NewStatus
    ) : IRequest<RequestResponse<int>>;

    public class CreateRefundHistoryCommandHandler : IRequestHandler<CreateRefundHistoryCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<RefundHistory> _refundHistoryRepository;

        public CreateRefundHistoryCommandHandler(IGeneralRepository<RefundHistory> refundHistoryRepository)
        {
            _refundHistoryRepository = refundHistoryRepository;
        }

        public async Task<RequestResponse<int>> Handle(CreateRefundHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = new RefundHistory
            {
                PaymentId = request.PaymentId,
                EnrollmentId = request.EnrollmentId,
                PreviousStatus = request.PreviousStatus,
                NewStatus = request.NewStatus,
                CreatedAt = DateTime.UtcNow
            };

            _refundHistoryRepository.Add(history);
            await _refundHistoryRepository.SaveChangesAsync();

            return RequestResponse<int>.Ok(history.Id, "Refund history recorded successfully.");
        }
    }
}

