using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseEntity
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
        public void SaveInclude(T entity, params string[] properties)
        {
            EntityEntry<T> entry;
            var localEntity = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

            // Check if entity is already tracked on the local 
            if(localEntity is null)
            {
                // Attach the entity to be tracked by ChangeTracker with Unchanged state
                entry = _context.Attach(entity);
            }
            else
            {
                // Get the entry of this tracked entity
                entry = _context.Entry(localEntity);

                // Set all properties values of tracked entity to properties values of provided entity to be updated
                entry.CurrentValues.SetValues(entity);
            }

            // Set ChangeTracker state to Modified for only properties of entity to be updated
            foreach(var propertyEntry in entry.Properties)
            {
                // Check if current property is Id to ignore
                if(propertyEntry.Metadata.IsPrimaryKey())
                    continue;
                
                // Change the ChangeTracker state of current property if it belongs to properties of entity
                // to be updated
                propertyEntry.IsModified = properties.Contains(propertyEntry.Metadata.Name);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

    }
}
