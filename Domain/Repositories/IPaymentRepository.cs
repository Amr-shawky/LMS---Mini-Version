namespace LMS___Mini_Version.Domain.Repositories;

public interface IPaymentRepository : IGeneralRepository<Payment>
{
    Task<Payment?> GetByEnrollmentIdAsync(int enrollmentId);

    Task<IEnumerable<Payment>> GetAllWithDetailsAsync();

    Task<bool> RefundPaymentAsync(int enrollmentId);

    Task<bool> IsPaidAsync(int enrollmentId);
}