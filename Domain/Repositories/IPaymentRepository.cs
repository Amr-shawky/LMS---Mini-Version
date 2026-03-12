using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories;

public interface IPaymentRepository : IGeneralRepository<Payment>
{
    // Get payment by enrollment (1-to-1)
    Task<Payment?> GetByEnrollmentIdAsync(int enrollmentId);

    // Get all with enrollment details
    Task<IEnumerable<Payment>> GetAllWithDetailsAsync();

    // Refund support — future endpoint
    Task<bool> RefundPaymentAsync(int enrollmentId);

    // Check if already paid
    Task<bool> IsPaidAsync(int enrollmentId);
}