using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);

        Task AddSavePointAsync(string savePointName, CancellationToken cancellationToken = default);
    }
}