using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Handler
{
    public class UpdatePaymentStatusHandler : IRequestHandler<UpdatePaymentStatusCommand, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Payment> _repository;

        public UpdatePaymentStatusHandler(IGeneralRepository<Payment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResult<bool>> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _repository.GetById(request.paymentId);
            if (payment == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.paymentNotExist);
            }
            payment.Status = request.Status;

            _repository.Update(payment);



            return RequestResult<bool>.Success(true);
        }
    }
}
