namespace LMS___Mini_Version.Domain.Repositories
{
    public interface IUnitOfWork
    {

        IGeneralRepository<T> GetRepository<T>() where T : class;

        Task<int> SaveChangesAsync();

    }
}
