using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS___Mini_Version.Infrastructure.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        private int _depth = 0;
        private string _currentSavePoint = string.Empty;

        public UnitOfWork(AppDbContext context) => _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default) 
        {
            var isOuterTransaction = _depth == 0;

            if (isOuterTransaction)
                await BeginTransactionAsync(cancellationToken);

            _depth++;

            try
            {
                await action();
                
                if (isOuterTransaction)
                {
                    await SaveChangesAsync(cancellationToken);
                    await CommitTransactionAsync(cancellationToken);
                }
            }
            catch
            {
                if (isOuterTransaction) 
                {
                    if (!string.IsNullOrEmpty(_currentSavePoint))
                        await RollbackToSavePointAsync(_currentSavePoint, cancellationToken);
                    else
                        await RollbackTransactionAsync(cancellationToken);
                }

                throw;
            }
            finally
            {
                _depth--;

                if (isOuterTransaction)
                {
                    await DisposeAsync();
                    ClearChangeTrackerMemoryState();
                    _transaction = null;
                }
            }

        }

        public async Task AddSavePointAsync(string savePointName, CancellationToken cancellationToken = default) 
        {
            if (_transaction is null)
                throw new InvalidOperationException("There is no opened transaction to create a new savepoint.");

            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CreateSavepointAsync(savePointName, cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is null)
                throw new InvalidOperationException("There is no transaction to dispose.");

            await _transaction.DisposeAsync();
        }

        private async Task BeginTransactionAsync(CancellationToken cancellationToken = default) 
            => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        private async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("There is no opened transaction to commit.");

            await _transaction.CommitAsync(cancellationToken);
        }

        private async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("There is no opened transaction to rollback.");

            await _transaction.RollbackAsync(cancellationToken);
        }

        private async Task RollbackToSavePointAsync(string savePointName, CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("There is no opened transaction to rollback to a specific savepoint.");

            await _transaction.RollbackToSavepointAsync(savePointName, cancellationToken);
        }

        private void ClearChangeTrackerMemoryState()
            => _context.ChangeTracker.Clear();
    }
}