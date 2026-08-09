using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record UpdatePaymentAmountCommand(int EnrollmentId, decimal NewAmount) : IRequest;


    public class UpdatePaymentAmountCommandHandler : IRequestHandler<UpdatePaymentAmountCommand, Unit>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;
        private readonly IMediator _mediator;

        public UpdatePaymentAmountCommandHandler(IGeneralRepository<Payment> paymentRepository , IMediator mediator)
        {
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        // not a correct way to fet the entity and update on it ! bluk update it efficient
        public async Task<Unit> Handle(UpdatePaymentAmountCommand request, CancellationToken cancellationToken)
        {
            var payment = await _mediator.Send(new GetPaymentByEnrollmentQuery(request.EnrollmentId));

            if (payment == null)
                throw new KeyNotFoundException($"there is no payment by this Enrollment");

                var paymentEntity = new Payment
                {
                    Amount = request.NewAmount,
                    Status = PaymentStatus.Pending
                };
                _paymentRepository.Update(paymentEntity);
            

            return Unit.Value;
        }
    }

}
