namespace LMS___Mini_Version.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {

        Task ExecuteAsync(Func<Task> action);

        Task AddSavePointAsync(string name);

        Task RollbackToSavePointAsync(string name);


        Task<int> SaveChangesAsync();
    }
}