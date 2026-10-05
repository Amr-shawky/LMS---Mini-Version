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
            => await _context.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id).ConfigureAwait(false);

        public IQueryable<T> GetTable()
            => _context.Set<T>();

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        #region entry

        //entry.State
        //entry.Properties
        //entry.Metadata
        //entry.CurrentValues
        //entry.OriginalValues
        #endregion
        public void SaveInclude(T entity, params string[] Includedproperties)
        {

            var localEntity = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

            EntityEntry entry;

            if (localEntity is null)
            {
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.ChangeTracker.Entries<T>().First(e => e.Entity.Id == entity.Id);
            }

            foreach (var property in entry.Properties)
            {
                if (Includedproperties.Contains(property.Metadata.Name))
                {
                    property.IsModified = true;
                }

                else
                {
                    property.IsModified = false;
                }
            }

        }
    }
}

#region save
/*
 
 var localEntity  = _context.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

            EntityEntry entry;

             if(localEntity is null)
            {
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.ChangeTracker.Entries<T>().First(e => e.Entity.Id == entity.Id);
            }

            foreach (var property in entry.Properties)
            {
                if (Includedproperties.Contains(property.Metadata.Name))
                {
                    property.IsModified = true;
                }

                else
                {
                    property.IsModified = false;
                }
            }
 */

#endregion