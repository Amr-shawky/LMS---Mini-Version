using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class refundPaymentCommandHandler : IRequestHandler<refundPaymentCommand>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;
        private readonly IGeneralRepository<RefundHistory> _refundHistoryRepo;

        public refundPaymentCommandHandler(
            IGeneralRepository<Payment> paymentRepo,
            IGeneralRepository<RefundHistory> refundHistoryRepo)
        {
            _paymentRepo = paymentRepo;
            _refundHistoryRepo = refundHistoryRepo;
        }

        public async Task<Unit> Handle(refundPaymentCommand request, CancellationToken ct)
        {
            var originalPayment = await _paymentRepo.GetAll()
                .Where(p => p.EnrollmentId == request.EnrollmentID)
                .OrderBy(p => p.Id)
                .FirstOrDefaultAsync(ct);

            if (originalPayment is null)
                return Unit.Value;  

            if (originalPayment.Status != PaymentStatus.Completed)
                return Unit.Value; 

            var previousStatus = originalPayment.Status;
            originalPayment.Status = PaymentStatus.Refunded;
            _paymentRepo.Update(originalPayment);

            await _paymentRepo.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
