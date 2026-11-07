using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.EnrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Handlers
{
    public class CreateRefundHistoryCommandHandler : IRequestHandler<CreateRefundHistoryCommand, RequestResponse>
    {
        private readonly IGeneralRepository<RefundHistory> _refundHistoryRepo;

        public CreateRefundHistoryCommandHandler(IGeneralRepository<RefundHistory> refundHistoryRepo)
        {
           _refundHistoryRepo = refundHistoryRepo;
        }
        public async Task<RequestResponse> Handle(CreateRefundHistoryCommand request, CancellationToken cancellationToken)
        {
            //throw new Exception("test savepoint");
            var history = new RefundHistory
            {
                PaymentId = request.PaymentId,
                EnrollmentId = request.EnrollmentId,
                PreviousStatus = request.PreviousStatus,
                NewStatus = request.NewStatus,
                CreatedAt = DateTime.UtcNow
            };

            _refundHistoryRepo.Add(history);
            await _refundHistoryRepo.SaveChangesAsync();
            return RequestResponse.Ok("Refund history recorded.");
        }
    }
}
