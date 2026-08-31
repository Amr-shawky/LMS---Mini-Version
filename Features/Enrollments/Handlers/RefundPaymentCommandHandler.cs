using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;
        private readonly IUnitOfWork _uow;

        public RefundPaymentCommandHandler(IGeneralRepository<Payment> PaymentRepo, IUnitOfWork uow)
        {
            _paymentRepo = PaymentRepo;
            _uow = uow;
        }
        public async Task<Unit> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepo.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.InrollmentId, cancellationToken);
            if (payment == null || payment.Status == PaymentStatus.Refunded)
                throw new KeyNotFoundException("From Refund Not Found");
            payment.Status = PaymentStatus.Refunded;
            _paymentRepo.Update(payment);
            await _uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
