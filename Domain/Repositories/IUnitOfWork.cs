namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
        bool HasActiveTransaction { get; }
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
    }
}
