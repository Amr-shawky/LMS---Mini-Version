using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Common
{
    public class GenericRepository<T,TKey> : IGenericRepository<T,TKey> where T:BaseEntity<TKey>
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetTable(int pageIndex,int pageSize)
        {
            return _context.Set<T>().Skip((pageIndex-1)*pageSize).Take(pageSize);
        }
        public async Task<T> GetByIdAsync(TKey id)
        {
            if (typeof(T) == typeof(Enrollment))
            {
                var enrollment =await _context.Set<Enrollment>().Include(e => e.Intern).Include(e => e.Track).FirstOrDefaultAsync(d => d.Id.Equals(id)) ;
                return enrollment as T;
            }
            if (typeof(T) == typeof(Intern))
            {
                var intern = await _context.Interns.Include(e => e.Track).FirstOrDefaultAsync(d => d.Id.Equals(id));
            }
            return await _context.Set<T>().FirstOrDefaultAsync(d => d.Id.Equals(id));
        }        
        public async Task AddAsync(T entity)
        {
          await  _context.Set<T>().AddAsync(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void SaveInclude(T entity,params string[] includedProperites)
        {
          var entry=  _context.Entry(entity);

            foreach(var properity in entry.Properties)
            {
                if (includedProperites.Contains(properity.Metadata.Name))
                {
                    properity.IsModified = true;
                }
                else
                {
                    properity.IsModified = false;
                }
            }
        }
        public async Task Delete(TKey id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<T>().Remove(entity);
        }

        

        

    }
}
