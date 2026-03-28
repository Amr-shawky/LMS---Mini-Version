namespace LMS___Mini_Version.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private ITrackRepository? _tracks;
        private IInternRepository? _interns;
        private IEnrollmentRepository? _enrollments;
        private IPaymentRepository? _payments;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITrackRepository Tracks
            => _tracks ??= new TrackRepository(_context);

        public IInternRepository Interns
            => _interns ??= new InternRepository(_context);

        public IEnrollmentRepository Enrollments
            => _enrollments ??= new EnrollmentRepository(_context);

        public IPaymentRepository Payments
            => _payments ??= new PaymentRepository(_context);

        /// <summary>
        /// Commits ALL staged changes across ALL repositories in a single DB transaction.
        /// </summary>
        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync().ConfigureAwait(false);
        
        public async Task<IDbContextTransaction> BeginTransactionAsync() =>
            await _context.Database.BeginTransactionAsync();
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
