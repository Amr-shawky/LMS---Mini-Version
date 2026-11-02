namespace LMS___Mini_Version.Common
{
    public interface IGenericRepository<T,TKey>
    {

        IQueryable<T> GetTable(int pageIndex, int pageSize);

        Task<T> GetByIdAsync(TKey id);       

        Task AddAsync(T entity);

        void Update(T entity);

        void SaveInclude(T entity, params string[] includedProperites);
        Task Delete(TKey id);

        
    }
}
