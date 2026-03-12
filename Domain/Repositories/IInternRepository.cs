using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories;

public interface IInternRepository : IGeneralRepository<Intern>
{
    Task<bool> IsEmailAddressTakenAsync(string email);

    Task<Intern?> GetWithTrackAsync(int id);
    
    Task<IEnumerable<Intern>> GetAllWithTrackAsync();
    
    Task<IEnumerable<Intern>> GetByTrackAsync(int trackId);
}