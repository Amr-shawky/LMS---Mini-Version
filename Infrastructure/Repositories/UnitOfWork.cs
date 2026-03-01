using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    /// <summary>
    /// [Trap 6 Fix] Unit of Work implementation wraps the DbContext and lazily creates
    /// repository instances. All repositories share the SAME DbContext instance,
    /// so changes across multiple entities are committed atomically via CompleteAsync().
    /// 
    /// Example flow:
    ///   1. Mediator calls _uow.Enrollments.Add(enrollment)   → staged in Change Tracker
    ///   2. Mediator calls _uow.Payments.Add(payment)          → staged in Change Tracker
    ///   3. Mediator calls _uow.CompleteAsync()                 → single SaveChangesAsync()
    ///   
    /// If step 2 throws, step 1 is automatically rolled back because SaveChanges was never called.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IGeneralRepository<Track>? _tracks;
        private IGeneralRepository<Intern>? _interns;
        private IGeneralRepository<Enrollment>? _enrollments;
        private IGeneralRepository<Payment>? _payments;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Lazy initialization — repositories are created only when first accessed
        public IGeneralRepository<Track> Tracks
            => _tracks ??= new GeneralRepository<Track>(_context);

        public IGeneralRepository<Intern> Interns
            => _interns ??= new GeneralRepository<Intern>(_context);

        public IGeneralRepository<Enrollment> Enrollments
            => _enrollments ??= new GeneralRepository<Enrollment>(_context);

        public IGeneralRepository<Payment> Payments
            => _payments ??= new GeneralRepository<Payment>(_context);

        /// <summary>
        /// Commits ALL staged changes across ALL repositories in a single DB transaction.
        /// </summary>
        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync().ConfigureAwait(false);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
