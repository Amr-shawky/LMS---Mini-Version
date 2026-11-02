using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using MediatR;

namespace LMS___Mini_Version.Features.Payment.RefunPayment.Command
{
    public record RefundPaymentCommand(int enrollmentId) : IRequest<bool>;
    public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RefundPaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
          var enrollment= await _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().GetByIdAsync(request.enrollmentId);
            if (enrollment == null) { return false; }
            enrollment.PaymentStatus = "refunded";

            _unitOfWork.GetRepository<LMS___Mini_Version.Domain.Entities.Enrollment, int>().SaveInclude(enrollment, nameof(enrollment.PaymentStatus));
           await _unitOfWork.SaveChagesAsync();
            return true;

        }
    }

}
