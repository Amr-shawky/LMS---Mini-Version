using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Common
{
    public interface IUnitOfWork
    {
        IGenericRepository<T, TKey> GetRepository<T,TKey>() where T:BaseEntity<TKey>;
        Task<int> SaveChagesAsync();
        Task ExecuteAsync(Func<Task> action);
        Task AddSavePoint(string savePoint);
    }
}
