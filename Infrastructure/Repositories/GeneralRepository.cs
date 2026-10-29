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
            // 1. Check if the entity is already tracked :
            var localEntity =  _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entry;

            //2. If the entity is not tracked, attach it to the context:
            if(localEntity == null)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            //3. If the entity is already tracked, update its values:
            else
            {
                entry = _context.Entry(localEntity);
                entry.CurrentValues.SetValues(entity);
            }

            // 4. Mark only the specified properties as modified :
            foreach(var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue; // Skip primary key properties

                // Mark the property as modified only if it's in the Includedproperties list
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
