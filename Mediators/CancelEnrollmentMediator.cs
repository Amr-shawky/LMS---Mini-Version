using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;

namespace LMS___Mini_Version.Mediators
{

    public class CancelEnrollmentMediator
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrackService _trackService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentMediator(
            IEnrollmentService enrollmentService,
            ITrackService trackService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _enrollmentService = enrollmentService;
            _trackService = trackService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MediatorResult> ExecuteAsync(int enrollmentId)
        {

            var enrollment = await _enrollmentService.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                return MediatorResult.Fail($"Enrollment with ID {enrollmentId} was not found.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return MediatorResult.Fail("This enrollment is already cancelled.");
            }

            var statusUpdated = await _enrollmentService
                .UpdateStatusAsync(enrollmentId, EnrollmentStatus.Cancelled)
                ;

            if (!statusUpdated)
            {
                return MediatorResult.Fail("Failed to update enrollment status.");
            }

            await _paymentService.RefundPaymentAsync(enrollmentId);

            await _unitOfWork.SaveChangesAsync();

            return MediatorResult.Succeed("Enrollment cancelled and payment refunded successfully.");
        }
    }
}
