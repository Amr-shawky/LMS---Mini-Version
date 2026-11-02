using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS___Mini_Version.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly Dictionary<string, object> _repo;
        private string _savePoint;
        private int _depth = 0;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repo = new Dictionary<string, object>();
        }
        

        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>
        {
            string EntityName = typeof(T).Name;
            if (_repo.ContainsKey(EntityName))
            {
                return _repo[EntityName] as IGenericRepository<T,TKey>; 
            }
            else
            {
                _repo.Add(EntityName, new GenericRepository<T, TKey>(_context));
                return _repo[EntityName] as IGenericRepository<T,TKey>;
            }
            
        }

        public Task<int> SaveChagesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public async Task ExecuteAsync(Func<Task> action)
        {
            bool isOuterLayer = _depth == 0;
            if (isOuterLayer)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }

            _depth++;

            try
            {
               await action.Invoke();
                if (isOuterLayer)
                {
                    await _context.SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
              
            }
            catch
            {
                if (isOuterLayer)
                {
                    if (!string.IsNullOrEmpty(_savePoint))
                    {
                       await _transaction.RollbackToSavepointAsync(_savePoint);
                    }
                    await _transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                _depth--;
                if (isOuterLayer)
                {
                    await _transaction.DisposeAsync();

                }
            }
        }

        public async Task AddSavePoint(string savePoint)
        {
            _savePoint = savePoint;
          await  _transaction.CreateSavepointAsync(savePoint);
        }
    }  
}
