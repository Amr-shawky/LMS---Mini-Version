using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Payments.Handler
{
    public class UpdatePaymentAmountHandler : IRequestHandler<UpdatePaymentAmountCommand, RequestResult<bool>>
    {
        readonly IGeneralRepository<Payment> _repository;
        public UpdatePaymentAmountHandler(IGeneralRepository<Payment> repository)
        {
            _repository = repository;
        }
        public async Task<RequestResult<bool>> Handle(UpdatePaymentAmountCommand request, CancellationToken cancellationToken)
        {
            var payment =await _repository.GetById(request.id);
            if (payment == null)
                return RequestResult<bool>.Failure(Domain.Enums.ErrorCode.paymentNotExist);
            payment.Amount = request.NewAmount;
            _repository.Update(payment);
            return RequestResult<bool>.Success(true);


            
        }
    }
}
