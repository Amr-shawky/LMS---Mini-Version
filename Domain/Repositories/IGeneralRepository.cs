namespace LMS___Mini_Version.Domain.Repositories
{
    public interface IGeneralRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        IQueryable<T> GetTable();

        void Add(T entity);

        void Update(T entity);

        void Delete(int id);
    }
}