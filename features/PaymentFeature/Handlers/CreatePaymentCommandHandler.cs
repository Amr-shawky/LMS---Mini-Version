using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.PaymentFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Infrastructure.Repositories;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, RequestResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGeneralRepository<Payment> paymentRepo;

        public CreatePaymentCommandHandler(IUnitOfWork unitOfWork , IGeneralRepository<Payment> paymentRepo)
        {
            this.unitOfWork = unitOfWork;
            this.paymentRepo = paymentRepo;
        }
        public  async Task<RequestResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {

             var paymentt = new Payment()
            {
               Amount = request.Amount,
              EnrollmentId = request.enrollementId,
              Method = request.PaymentMethod,
              PaymentDate = request.date,
              Status = request.PaymentStatus

            };
          paymentRepo.Add(paymentt);
            var res = await unitOfWork.SaveChangesAsync();
            return res > 0 ? RequestResponse.OK(200) : RequestResponse.Fail(400);
        }
    }
}
