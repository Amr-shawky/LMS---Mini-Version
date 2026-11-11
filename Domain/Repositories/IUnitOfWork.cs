using LMS___Mini_Version.features.shared;

namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
        
        Task<TResponse> ExecuteInTransactionAsync<TResponse>(
            Func<Task<TResponse>> operation,
            CancellationToken ct = default) where TResponse : RequestResponse;
    }
}
