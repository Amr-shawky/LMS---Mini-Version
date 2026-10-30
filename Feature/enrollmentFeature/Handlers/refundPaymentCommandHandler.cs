using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class refundPaymentCommandHandler:IRequestHandler<refundPaymentCommand, RequestResponse<int?>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;
        public refundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepo)=>
            _paymentRepo = paymentRepo;

        public async Task<RequestResponse<int?>> Handle(refundPaymentCommand request, CancellationToken cancellationToken)
        {
            //check if payment can refunded 
            var payment = await  _paymentRepo.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentID,cancellationToken);

            if (payment == null|| payment.Status != PaymentStatus.Completed)
                return RequestResponse<int?>.Ok(null);
  
            payment.Status = PaymentStatus.Refunded;

            return RequestResponse<int?>.Ok(payment.Id,"Refund the payment.");            
        }
    }
}
