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

        public void SaveInclude(T entity, params string[] includedProperties)
        {
            // 1. Check if the entity is already being tracked in m emory
            var localEntity = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id); 
            EntityEntry entry; 

            if (localEntity == null) { 
                entry =  _context.Attach(entity); 
            } 
            else { 
                entry = _context.Entry(localEntity); 
                entry.CurrentValues.SetValues (entity); 
            } 

            // 3. Loop through all properties of the entity
            foreach (var property in entry.Properties) 
            { 
                if (property.Metadata.IsPrimaryKey()) 
                    continue; 

                property.IsModified = includedProperties.Contains(property.Metadata.Name); 
            } 
        } 
        
    

    }
}
