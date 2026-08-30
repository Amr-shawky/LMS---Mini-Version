using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Payments.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Payments.Handlers
{
    public class UpdatePaymentAmountCommandHandler : IRequestHandler<UpdatePaymentAmountCommand>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;

        public UpdatePaymentAmountCommandHandler(IGeneralRepository<Payment> paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task Handle(UpdatePaymentAmountCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepo.GetTable()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentId, cancellationToken);

            if (payment != null)
            {
                payment.Amount = request.NewAmount;
                payment.Status = PaymentStatus.Pending;
                _paymentRepo.Update(payment);
            }
        }
    }
}
