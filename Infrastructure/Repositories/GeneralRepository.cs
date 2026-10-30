using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

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
        public void SaveInclude(T entity, params string[] Includedproperties)
        {
            if (entity.Id <= 0) throw new ArgumentException("A persisted ID is required.");
            if (Includedproperties is null || Includedproperties.Length == 0)
                throw new ArgumentException("Specify at least one property.");

            var tracked = _context.Set<T>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (tracked is null)
            {
                tracked = entity;
                _context.Set<T>().Attach(tracked);
            }

            var entry = _context.Entry(tracked);
            foreach (var included in Includedproperties.Distinct(StringComparer.Ordinal))
            {
                var property = entry.Property(included); // rejects unknown property names
                if (property.Metadata.IsPrimaryKey())
                    throw new ArgumentException("Primary keys cannot be selectively updated.");
                if (!ReferenceEquals(tracked, entity))
                    property.CurrentValue = _context.Entry(entity).Property(included).CurrentValue;
            }

            foreach (var property in entry.Properties)
                property.IsModified = Includedproperties.Contains(property.Metadata.Name);
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

    }
}
