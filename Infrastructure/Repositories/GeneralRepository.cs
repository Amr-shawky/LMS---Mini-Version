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

        /// <summary>
        /// Marks ONLY the listed scalar properties as Modified, without overwriting the entire row.
        /// This is the correct way to do a partial update: only the named columns are included
        /// in the SQL UPDATE statement that EF Core will generate.
        ///
        /// Why not call Update(entity)?
        ///   Update() marks EVERY property as Modified, so EF writes every column even if unchanged.
        ///   SaveInclude() is precise — only the columns you name are written.
        ///
        /// Detached entities (common when the context uses NoTracking):
        ///   We attach the entity first so EF can track it, then mark each property individually.
        /// </summary>
        public void SaveInclude(T entity, params string[] Includedproperties)
        {
            // Validate that every requested property actually exists on the entity type.
            var entityType = typeof(T);
            foreach (var propertyName in Includedproperties)
            {
                var prop = entityType.GetProperty(propertyName);
                if (prop == null)
                    throw new InvalidOperationException(
                        $"Property '{propertyName}' does not exist on type '{entityType.Name}'. " +
                        "Check for typos in the property name passed to SaveInclude.");
            }

            // If the entity is not yet tracked (NoTracking context), attach it in Unchanged state
            // so we can then selectively mark properties as Modified.
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }

            // Mark only the specified properties as Modified. EF Core will include ONLY
            // these columns in the SQL UPDATE statement, leaving all others untouched.
            foreach (var propertyName in Includedproperties)
            {
                entry.Property(propertyName).IsModified = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

    }
}
