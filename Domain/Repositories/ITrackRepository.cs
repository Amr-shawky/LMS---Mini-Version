using LMS___Mini_Version.Domain.Entities;

namespace LMS___Mini_Version.Domain.Repositories;

public interface ITrackRepository : IGeneralRepository<Track>
{
    // 🔍 Capacity check
    Task<bool> CheckCapacityAsync(int trackId);

    // 🔍 Get active tracks only
    Task<IEnumerable<Track>> GetActiveTracksAsync();

    // 🔍 Get with enrollment count for display
    Task<Track?> GetByIdWithEnrollmentCountAsync(int id);

    // 🔍 Name uniqueness check
    Task<bool> IsNameTakenAsync(string name);
    
}