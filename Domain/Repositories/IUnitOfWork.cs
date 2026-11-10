using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>()
           where TEntity : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}
