namespace LMS___Mini_Version.Infrastructure.Repositories;

public class PaymentRepository: GeneralRepository<Payment>, IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // ✅ 1-to-1 lookup by enrollment
    public async Task<Payment?> GetByEnrollmentIdAsync(int enrollmentId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId);
    }

    // ✅ All payments with enrollment + intern + track context
    public async Task<IEnumerable<Payment>> GetAllWithDetailsAsync()
    {
        return await _context.Payments
            .Include(p => p.Enrollment)
            .ThenInclude(e => e.Intern)
            .Include(p => p.Enrollment)
            .ThenInclude(e => e.Track)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    // ✅ Refund — stages change only, UoW commits
    public async Task<bool> RefundPaymentAsync(int enrollmentId)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId);

        if (payment == null) return false;
        if (payment.Status == PaymentStatus.Refunded) return false;

        payment.Status = PaymentStatus.Refunded; // ✅ Staged only
        return true;
    }

    // ✅ Safety check before processing payment
    public async Task<bool> IsPaidAsync(int enrollmentId)
    {
        return await _context.Payments
                    .AnyAsync(p => p.EnrollmentId == enrollmentId
                             && p.Status == PaymentStatus.Completed);
    }
}