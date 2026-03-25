using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.DTO_S.PayementsDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Handler
{
    public class StageNewPaymentHandler(IGeneralRepository<Payment> _repository) : IRequestHandler<StageNewPaymentCommand, RequestResult<PaymentDTO>>
    {
        public async Task<RequestResult<PaymentDTO>> Handle(StageNewPaymentCommand request, CancellationToken cancellationToken)
        {
            var fees = request.Amount;
            if (fees <= 0)
            {
                return  RequestResult<PaymentDTO>.Failure(ErrorCode.NoFees);
            }
            var payment = new Payment()
            {
                Amount = fees,
                EnrollmentId = request.EnrollmentId,
                PaymentDate=DateTime.UtcNow,
                Method = request.Method,
                Status=PaymentStatus.Pending,
            };
            _repository.Add(payment);

            return  RequestResult<PaymentDTO>.Success(payment.ToPaymentDto());
          
               
        }
    }
}
