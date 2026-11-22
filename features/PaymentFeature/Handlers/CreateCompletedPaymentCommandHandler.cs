using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.PaymentFeature.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.Handlers
{
    public class CreateCompletedPaymentCommandHandler : IRequestHandler<CreateCompletedPaymentCommand, RequestResponse>
    {
        private readonly IGeneralRepository<Payment> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompletedPaymentCommandHandler(IGeneralRepository<Payment> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(CreateCompletedPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                EnrollmentId = request.EnrollmentId,
                Amount = request.TrackFees,
                Status = PaymentStatus.Completed,
                PaymentDate = DateTime.UtcNow,
            };

            _repository.Add(payment);

            await _unitOfWork.SaveChangesAsync();

            return RequestResponse.OK();
        }
    }
}
