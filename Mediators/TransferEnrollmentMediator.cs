using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;

namespace LMS___Mini_Version.Mediators
{

    public class TransferEnrollmentMediator
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ITrackService _trackService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentMediator(
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

        public async Task<MediatorResult> ExecuteAsync(int enrollmentId, int newTrackId)
        {

            var enrollment = await _enrollmentService.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                return MediatorResult.Fail($"Enrollment with ID {enrollmentId} was not found.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return MediatorResult.Fail("Cannot transfer a cancelled enrollment.");
            }

            if (enrollment.TrackId == newTrackId)
            {
                return MediatorResult.Fail("The intern is already enrolled in this track.");
            }

            var newTrack = await _trackService.GetByIdAsync(newTrackId);
            if (newTrack == null)
            {
                return MediatorResult.Fail($"Target Track with ID {newTrackId} was not found.");
            }
            if (!newTrack.IsActive)
            {
                return MediatorResult.Fail($"Target Track '{newTrack.Name}' is not currently active.");
            }

            var hasCapacity = await _trackService.CheckCapacityAsync(newTrackId);
            if (!hasCapacity)
            {
                return MediatorResult.Fail($"Target Track '{newTrack.Name}' has reached its maximum capacity.");
            }

            var trackUpdated = await _enrollmentService
                .UpdateTrackAsync(enrollmentId, newTrackId)
                ;

            if (!trackUpdated)
            {
                return MediatorResult.Fail("Failed to update the enrollment's track.");
            }

            if (newTrack.Fees > 0)
            {
                await _paymentService
                    .UpdatePaymentAmountAsync(enrollmentId, newTrack.Fees)
                    ;
            }

            await _unitOfWork.SaveChangesAsync();

            return MediatorResult.Succeed(
                $"Enrollment transferred to '{newTrack.Name}' successfully. New fees: {newTrack.Fees:C}.");
        }
    }
}
