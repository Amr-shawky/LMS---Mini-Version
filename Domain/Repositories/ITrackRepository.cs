namespace LMS___Mini_Version.Domain.Repositories;

public interface ITrackRepository : IGeneralRepository<Track>
{
    Task<bool> CheckCapacityAsync(int trackId);

    Task<IEnumerable<Track>> GetActiveTracksAsync();

    Task<Track?> GetByIdWithEnrollmentCountAsync(int id);

    Task<bool> IsNameTakenAsync(string name);
    
    Task<bool> IsTrackActiveAsync(int id);
    
}