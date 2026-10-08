using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.paymentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.paymentFeature.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public CreatePaymentCommandHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                EnrollmentId = request.EnrollmentId,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = request.Method,
                Status = request.Status
            };

            _paymentRepository.Add(payment);
            await _paymentRepository.SaveChangesAsync();

            return RequestResponse<int>.Created(payment.Id);
        }
    }
}
