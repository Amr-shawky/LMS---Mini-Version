using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LMS___Mini_Version.Infrastructure.Repositories
{

    public class GeneralRepository<T> : IGeneralRepository<T> where T : baseEntity
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

        public void SaveIncludes(T entity, params string[] includes)
        {
            var local = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);
            EntityEntry<T> entry;
            if (local is null)
            {
                entry = _context.Entry(entity);
                
            }
            else
            {
                entry = _context.ChangeTracker.Entries<T>().First(e => e.Entity.Id == entity.Id);
            }
            entry.State = EntityState.Modified;
            foreach (var include in includes)
            {
                entry.Property(include).IsModified = true;
            }
        }        

        public async Task<bool> ExistsAsync(int id)
        => await _context.Set<T>().AnyAsync(e => e.Id == id);
    }
}
