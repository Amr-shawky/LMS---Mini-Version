namespace LMS___Mini_Version.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {

        Task<int> SaveChangesAsync();
    }
}
