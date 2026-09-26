using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
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
            => await _context.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id).ConfigureAwait(false);

        public IQueryable<T> GetTable()
            => _context.Set<T>();

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public Task SaveIncludeAsync(T entity, params string[] properties)
        {
            var entityEntry = _context.Entry(entity);
            var keyProperty = entityEntry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name ?? "Id";
            var entityKeyValue = entityEntry.Property(keyProperty).CurrentValue;

            var local = _context.Set<T>().Local
                .FirstOrDefault(e => _context.Entry(e).Property(keyProperty).CurrentValue?.Equals(entityKeyValue) == true);

            EntityEntry<T> entry = local == null
                ? _context.Attach(entity)
                : _context.Entry(local);

            if (local != null)
            {
                _context.Entry(local).CurrentValues.SetValues(entity);
            }

            foreach (var property in properties)
            {
                entry.Property(property).IsModified = true;
            }

            return Task.CompletedTask;
        }
    }
}