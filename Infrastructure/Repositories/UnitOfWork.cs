using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LMS___Mini_Version.Infrastructure.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

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
        // do not work
        public async Task SaveIncludesAsync<TEntity>(TEntity entity, params string[] includes) where TEntity : baseEntity
        {
            var local = _context.Set<TEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
            EntityEntry entityEntry;
            if(local is null)
            {
                entityEntry = _context.Entry(entity);
            }
            else
            {
                entityEntry = _context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Id == entity.Id);
            }

            entityEntry.State = EntityState.Modified;
            foreach(var include in includes)
            {
                entityEntry.Reference(include).IsModified = true;
            }
            await _context.SaveChangesAsync();

        }
    }
}
