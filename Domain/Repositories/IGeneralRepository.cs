using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories
{
    public interface IGeneralRepository<T> where T : baseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> GetAll();

        Task<T?> GetByIdAsync(int id);

        IQueryable<T> GetTable();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void SaveInclude(T entity, params string[] properties);
    }
}