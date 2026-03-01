using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{
    /// <summary>
    /// [Trap 5 Fix] Service layer for Payment — single-entity Steps only.
    /// </summary>
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto?> GetByEnrollmentAsync(int enrollmentId);

        /// <summary>
        /// Creates a payment record in memory (staged in Change Tracker).
        /// Does NOT call SaveChanges — committed atomically by UoW.
        /// </summary>
        Task<PaymentDto> CreatePaymentAsync(PaymentDto dto);
    }
}
