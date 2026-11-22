using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS___Mini_Version.Infrastructure.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public bool HasActiveTransaction => _transaction != null;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
            => _transaction = await _context.Database.BeginTransactionAsync(ct);


        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _transaction!.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        public async Task RollbackAsync(CancellationToken ct = default)
        {
            await _transaction!.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
