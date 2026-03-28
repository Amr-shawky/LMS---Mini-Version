using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.CQRS.Enrollments.Services
{
    public class EnrollmentCanceller
    {
        private readonly IUnitOfWork _unitOf;
        public EnrollmentCanceller(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<EnrollmentResultDto> CancelAsync(int enrollmentId)
        {
            await using var transaction = await _unitOf.BeginTransactionAsync();

            try
            {
                var enrollment = await _unitOf.Enrollments.GetByIdAsync(enrollmentId);


                enrollment!.Status = EnrollmentStatus.Cancelled;
                _unitOf.Enrollments.Update(enrollment);

                //  Refund
                Payment? payment = null;
                var existingPayment = await _unitOf.Payments.GetByEnrollmentIdAsync(enrollmentId);

                if (existingPayment != null)
                {
                    var refunded = await _unitOf.Payments.RefundPaymentAsync(enrollmentId);
                    if (refunded)
                        payment = existingPayment;
                }

                await _unitOf.CompleteAsync();
                await transaction.CommitAsync();

                return EnrollmentResultDto.Succeed(enrollment.ToDto(), payment?.ToDto());
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
