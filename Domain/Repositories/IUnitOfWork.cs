namespace LMS___Mini_Version.Domain.Repositories
{
   public interface IUnitOfWork : IDisposable
    {
        ITrackRepository Tracks { get; }
        IInternRepository Interns { get; }
        IEnrollmentRepository Enrollments { get; }
        IPaymentRepository Payments { get; }
        
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
