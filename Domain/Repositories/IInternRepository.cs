namespace LMS___Mini_Version.Domain.Repositories;

public interface IInternRepository : IGeneralRepository<Intern>
{
    Task<bool> IsEmailAddressTakenAsync(string email);

    Task<Intern?> GetWithTrackAsync(int internId);
    
    Task<IEnumerable<Intern>> GetAllWithTrackAsync();
    
    Task<IEnumerable<Intern>> GetAllByTrackIdAsync(int trackId);
}