using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{

    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto?> GetByEnrollmentAsync(int enrollmentId);

        Task<PaymentDto> CreatePaymentAsync(PaymentDto dto);

        Task<bool> RefundPaymentAsync(int enrollmentId);

        Task<bool> UpdatePaymentAmountAsync(int enrollmentId, decimal newAmount);
    }
}
