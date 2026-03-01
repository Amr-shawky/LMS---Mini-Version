namespace LMS___Mini_Version.Domain.Repositories
{
    /// <summary>
    /// [Trap 1 Fix] Generic repository interface that abstracts the data access layer.
    /// Controllers will never see DbContext — only Services use repositories via IUnitOfWork.
    /// 
    /// [Trap 3 Fix] All methods are async to prevent thread-pool starvation under load.
    /// 
    /// [Trap 4 Fix] GetTableAsync() returns IQueryable so the Service layer can compose
    /// database-side filters before executing. GetAllAsync() eagerly executes and returns IEnumerable.
    /// 
    /// [Trap 6 Fix] No SaveChanges here — that responsibility belongs to IUnitOfWork.CompleteAsync().
    /// </summary>
    public interface IGeneralRepository<T> where T : class
    {
        // Returns fully-materialized list (query already executed)
        Task<IEnumerable<T>> GetAllAsync();

        // Uses FindAsync to leverage the Change Tracker (avoids redundant DB hits)
        Task<T?> GetByIdAsync(int id);

        // Returns IQueryable so the Service layer can add .Where() filters that run in the DB
        IQueryable<T> GetTable();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}