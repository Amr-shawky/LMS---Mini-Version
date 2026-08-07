using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class TransferEnrollmentCommandHandler : IRequestHandler<TransferEnrollmentCommand,bool>
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentCommandHandler(
            IEnrollmentService enrollmentService, 
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(TransferEnrollmentCommand request, CancellationToken cancellationToken)
        {
          var enrollmentExists=  await _enrollmentService.UpdateTrackAsync(request.EnrollmentId, request.NewTrackId);
            if(!enrollmentExists) return false;
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
