using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    /// <summary>
    /// [Trap 3 Fix] All I/O operations use async/await to free up threads.
    /// [Trap 4 Fix] GetAll() returns IQueryable so filters execute in the DB, not in memory.
    /// [Trap 6 Fix] SaveChanges() has been REMOVED from every method.
    ///              Changes are only staged in the Change Tracker until UnitOfWork.SaveChangesAsync() is called.
    /// </summary>
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GeneralRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();


        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}