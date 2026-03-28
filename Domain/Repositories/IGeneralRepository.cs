namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IGeneralRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        IQueryable<T> GetTable();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
        
    }
}