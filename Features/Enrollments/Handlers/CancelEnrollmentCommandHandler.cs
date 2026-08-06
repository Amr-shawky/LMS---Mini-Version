using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand,Unit>
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IPaymentService _paymentService;

        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentCommandHandler(IEnrollmentService enrollmentService,IPaymentService paymentService,IUnitOfWork unitOfWork)
        {
            _enrollmentService = enrollmentService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
        {

            var enrollment = await _enrollmentService.GetByIdAsync(request.EnrollmentId);
            if (enrollment == null)        
                throw new Exception("Enrollment is not found");

            if (enrollment.Status == EnrollmentStatus.Cancelled)           
                throw new Exception("This enrollment is cancelled");
            

            var updatedResult = await _enrollmentService.UpdateStatusAsync(request.EnrollmentId,EnrollmentStatus.Cancelled);

            if(!updatedResult)
                throw new Exception("Faild Update Enrollmetn Status");

            await _paymentService.RefundPaymentAsync(request.EnrollmentId);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
