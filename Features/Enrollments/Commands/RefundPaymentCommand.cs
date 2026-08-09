using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
        public record RefundPaymentCommand(int EnrollmentId) : IRequest;


    public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, Unit>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;
        private readonly IMediator _mediator;

        public RefundPaymentCommandHandler(IGeneralRepository<Payment> paymentRepository, IMediator mediator)
        {
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _mediator.Send(new GetPaymentByEnrollmentQuery(request.EnrollmentId));

            if (payment == null)
                throw new KeyNotFoundException($"there is no payment by this Enrollment");

                var paymentEntity = new Payment{ Status = PaymentStatus.Refunded };
                _paymentRepository.Update(paymentEntity);

            return Unit.Value;
        }
    }


}
