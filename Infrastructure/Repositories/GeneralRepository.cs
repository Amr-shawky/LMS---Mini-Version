using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GeneralRepository(AppDbContext context)
        {
            _context = context;
        }

        // مشكلة 3: تنفيذ الـ Query فوراً في الـ Repository (Memory Exhaustion)
        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

        public T GetById(int id) => _context.Set<T>().Find(id);

        public IQueryable<T> GetTable() => _context.Set<T>();

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            // مشكلة 4: مناداة SaveChanges هنا بتمنعنا من استخدام الـ Unit of Work لاحقاً
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}