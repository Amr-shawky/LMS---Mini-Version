using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class CreateRefundedPaymentHistoryCommandHandler:IRequestHandler<CreateRefundedPaymentHistoryCommand,RequestResponse<int>>
    {
        private readonly IGeneralRepository<RefundHistory> _refundPaymentRepo;

        public CreateRefundedPaymentHistoryCommandHandler(IGeneralRepository<RefundHistory> refundPaymentRepo) =>
            _refundPaymentRepo = refundPaymentRepo;

        public async Task<RequestResponse<int>> Handle(CreateRefundedPaymentHistoryCommand request, CancellationToken cancellationToken)
        {
            var refund = new RefundHistory
            {
                EnrollmentId = request.EnrollmentId,
                PaymentId = request.PaymentId,
                CreateAt = DateTime.UtcNow,
            };

            _refundPaymentRepo.Add(refund);

            await _refundPaymentRepo.SaveChangesAsync(cancellationToken);

            return RequestResponse<int>.Created(refund.Id);

        }
    }
}
