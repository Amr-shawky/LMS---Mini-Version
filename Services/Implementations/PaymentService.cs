using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{
    /// <summary>
    /// [Trap 5 Fix] Single-entity Steps for Payment.
    /// [Trap 6 Fix] Only stages changes — no SaveChanges.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments
                .GetTable()
                .Include(p => p.Enrollment)
                .ToListAsync()
                .ConfigureAwait(false);

            return payments.Select(p => p.ToDto());
        }

        public async Task<PaymentDto?> GetByEnrollmentAsync(int enrollmentId)
        {
            var payment = await _unitOfWork.Payments
                .GetTable()
                .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId)
                .ConfigureAwait(false);

            return payment?.ToDto();
        }

        /// <summary>
        /// Stages a new Payment entity in the Change Tracker.
        /// </summary>
        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto dto)
        {
            var entity = new Payment
            {
                EnrollmentId = dto.EnrollmentId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = dto.Method,
                Status = PaymentStatus.Pending
            };

            _unitOfWork.Payments.Add(entity);
            return entity.ToDto();
        }
    }
}
