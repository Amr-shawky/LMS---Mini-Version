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
            var tracked = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);
            var entry = tracked is null ? _context.Attach(entity) : _context.Entry(tracked);

            foreach (var name in includes)
            {
                var newValue = typeof(T).GetProperty(name)!.GetValue(entity);
                entry.Property(name).CurrentValue = newValue;   
                entry.Property(name).IsModified = true;         
            }
        }        

        public async Task<bool> ExistsAsync(int id)
        => await _context.Set<T>().AnyAsync(e => e.Id == id);
    }
}
