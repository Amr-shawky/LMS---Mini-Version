using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, RequestResponse>
    {
        IGeneralRepository<Payment> _paymentRepository;
        public CreatePaymentHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<RequestResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            Payment payment = _paymentRepository.GetAll().FirstOrDefault(p => p.EnrollmentId == request.enrollmentId);
            if(payment is null)
            { 
                Payment newPayment = new Payment
                {
                    EnrollmentId = request.enrollmentId,
                    Method = request.paymentMethod,
                    PaymentDate = DateTime.Now,
                    Status = PaymentStatus.Completed
                };
                _paymentRepository.Add(newPayment);
                return RequestResponse.OK();
            }
            payment.Method = request.paymentMethod;
            payment.PaymentDate = DateTime.Now;
            payment.Status = PaymentStatus.Completed;
            _paymentRepository.Update(payment);
            return RequestResponse.OK();
        }
    
    }
}
