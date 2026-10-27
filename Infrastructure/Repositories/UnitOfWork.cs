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
        private string? _currentSavePoint;

        public UnitOfWork(AppDbContext context) => _context = context;

        public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            var isOuterTransaction = _depth == 0;

            if (isOuterTransaction)
            {
                if(_context.Database.CurrentTransaction != null)
                {
                    throw new InvalidOperationException("A transaction is already in progress.");
                }
                _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            }

            _depth++;

            try
            {
                await action(cancellationToken);
                
                if (isOuterTransaction)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    await _transaction!.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                if (isOuterTransaction && _transaction != null)
                {
                    try
                    {
                        await _transaction.RollbackAsync(CancellationToken.None);
                    }
                    catch
                    {
                        // Log the rollback failure if necessary
                    }
                }
                    throw;
            }

            finally
            {
                _depth--;
                if (isOuterTransaction)
                {
                    if(_transaction != null)
                    {
                        await _transaction.DisposeAsync();
                        _transaction = null;
                    }
                    _currentSavePoint = null;
                }
            }
        }


        public async Task AddSavePointAsync(string savePointName, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(savePointName);
            if(_transaction == null)
            {
                throw new InvalidOperationException("No active transaction to create a savepoint.");
            }
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CreateSavepointAsync(savePointName,cancellationToken);
            _currentSavePoint = savePointName;
        }

        public async Task RollbackToSavePointAsync(string savePointName, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(savePointName);
            if(_transaction == null)
            {
                throw new InvalidOperationException("No active transaction to rollback to a savepoint.");
            }
            await _transaction.RollbackToSavepointAsync(savePointName, cancellationToken);
        }


        // Transaction methods
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

    }
}