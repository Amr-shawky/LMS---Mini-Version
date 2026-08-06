using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    /// <summary>
    /// [Trap 3 Fix] All I/O operations use async/await to free up threads.
    /// [Trap 4 Fix] GetTable() returns IQueryable so filters execute in the DB, not in memory.
    /// [Trap 6 Fix] SaveChanges() has been REMOVED from every method.
    ///              Changes are only staged in the Change Tracker until UnitOfWork.CompleteAsync() is called.
    /// </summary>
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GeneralRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Materializes the entire table into memory. Use GetTable() + filters for large datasets.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync().ConfigureAwait(false);

        /// <summary>
        /// Uses FindAsync which checks the Change Tracker first, avoiding an extra DB round-trip
        /// if the entity was already loaded in this request scope.
        /// </summary>
        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id).ConfigureAwait(false);

        /// <summary>
        /// [Trap 4 Fix] Returns IQueryable — the query is NOT executed here.
        /// The Service layer adds .Where() filters and calls .ToListAsync() itself,
        /// ensuring the filtering happens in SQL, not in C# memory.
        /// </summary>
        public IQueryable<T> GetTable()
            => _context.Set<T>();

        // No SaveChanges() — changes are staged in the Change Tracker.
        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}