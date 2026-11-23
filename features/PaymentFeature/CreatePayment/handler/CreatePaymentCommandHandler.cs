using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.PaymentFeature.CreatePayment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.PaymentFeature.CreatePayment.handler
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Payment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IGeneralRepository<Payment> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (request.EnrollmentId <= 0)
            {
                errors.Add("EnrollmentId",new[] { "Enrollment ID must be greater than zero." });
            }
            if (request.Amount < 0)
            {
                errors.Add("Amount",new[] { "Payment amount cannot be negative." });
            }
            if (!Enum.IsDefined(typeof(PaymentMethod), request.Method))
            {
                errors.Add("PaymentMethod",new[] { "Invalid payment method." });
            }
            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }
            var paymentExists = await _repository.GetAll()
                .AnyAsync(p => p.EnrollmentId == request.EnrollmentId,cancellationToken);

            if(paymentExists)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Enrollment", new[] { "Payment already exists for this enrollment." } } });
            }

            var payment = new Payment
            {
                EnrollmentId = request.EnrollmentId,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = request.Method,
                Status = PaymentStatus.Completed
            };

            _repository.Add(payment);
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<int>.OK(payment.Id, 201);
        }
    }
}
