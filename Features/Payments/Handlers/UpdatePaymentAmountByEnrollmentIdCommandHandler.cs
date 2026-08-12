using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Payments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Payments.Handlers
{
    public class UpdatePaymentAmountByEnrollmentIdCommandHandler : IRequestHandler<UpdatePaymentAmountByEnrollmentIdCommand, Unit>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;

        public UpdatePaymentAmountByEnrollmentIdCommandHandler(IGeneralRepository<Payment> paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<Unit> Handle(UpdatePaymentAmountByEnrollmentIdCommand request, CancellationToken cancellationToken)
        {
           var payment=await _paymentRepo.GetTable().FirstOrDefaultAsync(p => p.EnrollmentId == request.enrollmentId);
            if(payment is null)
                return Unit.Value;
            payment.Amount = request.newAmount;
            payment.Status = PaymentStatus.Pending;
            _paymentRepo.Update(payment);
            return Unit.Value;
        }
    }
}
