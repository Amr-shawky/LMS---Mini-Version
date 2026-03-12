using LMS___Mini_Version.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS___Mini_Version.Domain.Repositories
{
    /// <summary>
    /// [Trap 6 Fix] Unit of Work ensures that all changes across multiple repositories
    /// are committed in a single atomic transaction via CompleteAsync().
    /// No individual repository calls SaveChanges — they only stage changes in memory.
    /// The Mediator (or Controller for simple CRUD) calls CompleteAsync() once at the end.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ITrackRepository Tracks { get; }
        IInternRepository Interns { get; }
        IEnrollmentRepository Enrollments { get; }
        IPaymentRepository Payments { get; }
        
        Task<IDbContextTransaction> BeginTransactionAsync();
        /// <summary>
        /// Commits all staged changes to the database in one transaction.
        /// </summary>
        Task<int> CompleteAsync();
    }
}
