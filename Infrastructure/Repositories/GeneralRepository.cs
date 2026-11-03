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

        public void SaveInclude(T entity, params string[] Includedproperties)
        {
            // 1. Check if the entity is already being tracked in memory
            var localEntity = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);
            EntityEntry entry;

            if (localEntity == null)
            {
                // 2a. If not tracked, attach the new entity.
                // Attach sets all properties to 'Unchanged' state initially.
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            else
            {
                // 2b. If already tracked, copy the new values into the tracked entity to avoid tracking conflicts.
                entry = _context.Entry(localEntity);
                _context.Entry(localEntity).CurrentValues.SetValues(entity);
            }

            // 3. Loop through all properties of the entity
            foreach (var property in entry.Properties)
            {
                // Ignore the primary key (you cannot update an ID)
                if (property.Metadata.IsPrimaryKey())
                    continue;

                // 4. Set the 'IsModified' flag ONLY for the properties passed in the array
                property.IsModified = Includedproperties.Contains(property.Metadata.Name);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

    }
}
