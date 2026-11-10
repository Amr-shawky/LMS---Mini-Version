using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
        Task SaveIncludesAsync<TEntity>(TEntity entity, params string[] includes) where TEntity : baseEntity;
    }
}
