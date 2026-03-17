using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;

namespace LMS___Mini_Version.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext _appDbContext) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        public IGeneralRepository<T> GetRepository<T>() where T : class
        {
            var entityType = typeof(T);

            if (_repositories.ContainsKey(entityType))
            {
                return (GeneralRepository<T>)_repositories[typeof(T)];
            }

            var repo = new GeneralRepository<T>(_appDbContext);

            _repositories[entityType] = repo;

            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
