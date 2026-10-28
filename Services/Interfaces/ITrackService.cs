using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{

    public interface ITrackService
    {
        Task<IEnumerable<TrackDto>> GetAllAsync();
        Task<TrackDto?> GetByIdAsync(int id);
        Task<TrackDto> CreateAsync(TrackDto dto);
        Task<bool> UpdateAsync(int id, TrackDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> CheckCapacityAsync(int trackId);
    }
}
