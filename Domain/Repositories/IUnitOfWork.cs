using LMS___Mini_Version.Domain.Entities;

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
        IGeneralRepository<Track> Tracks { get; }
        IGeneralRepository<Intern> Interns { get; }
        IGeneralRepository<Enrollment> Enrollments { get; }
        IGeneralRepository<Payment> Payments { get; }

        /// <summary>
        /// Commits all staged changes to the database in one transaction.
        /// </summary>
        Task<int> CompleteAsync();
    }
}
