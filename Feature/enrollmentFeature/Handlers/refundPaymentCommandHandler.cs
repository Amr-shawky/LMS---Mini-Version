using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class refundPaymentCommandHandler : IRequestHandler<refundPaymentCommand, Unit>
    {
        private readonly IGeneralRepository<Payment> _paymentrepo;
        private readonly IUnitOfWork _unitOfWork;
        
        public refundPaymentCommandHandler(IGeneralRepository<Payment> paymentrepo, IUnitOfWork unitOfWork)
        {
            _paymentrepo = paymentrepo;
            _unitOfWork = unitOfWork;

        }
        public async Task<Unit> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            var paymententity = await _paymentrepo.GetAll()
                .FirstOrDefaultAsync(p=>p.EnrollmentId == request.EnrollmentID);
            
            if(paymententity == null)
            {
                throw new KeyNotFoundException($"the Payment doesnt contain contain " +
                    $"this enrollment ID {request.EnrollmentID}");
            }

            paymententity.Status = PaymentStatus.Refunded;

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
