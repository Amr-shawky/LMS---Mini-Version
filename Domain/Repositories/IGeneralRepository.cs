namespace LMS___Mini_Version.Domain.Repositories
{
    /// <summary>
    /// [Trap 1 Fix] Generic repository interface that abstracts the data access layer.
    /// Controllers will never see DbContext — only Services use repositories via IUnitOfWork.
    /// 
    /// [Trap 3 Fix] All methods are async to prevent thread-pool starvation under load.
    /// 
    /// [Trap 4 Fix] GetAllAsync() returns IQueryable so the Service layer can compose
    /// database-side filters before executing. GetAllAsync() eagerly executes and returns IEnumerable.
    /// 
    /// [Trap 6 Fix] No SaveChanges here — that responsibility belongs to IUnitOfWork.SaveChangesAsync().
    /// </summary>
    public interface IGeneralRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> GetAll();

        Task<T?> GetByIdAsync(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}