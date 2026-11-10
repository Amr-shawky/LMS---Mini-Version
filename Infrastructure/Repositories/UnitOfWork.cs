using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;

namespace LMS___Mini_Version.Infrastructure.Repositories
{

    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private readonly Dictionary<Type, object> Repositories = [];
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var entityType = typeof(TEntity);
            if (Repositories.TryGetValue(entityType, value: out var repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            var newRepo = new GenericRepository<TEntity>(_context);
            Repositories[entityType] = newRepo;
            return newRepo;
        }


        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
