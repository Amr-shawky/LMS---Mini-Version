using Microsoft.EntityFrameworkCore.Storage;

namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    }
}
