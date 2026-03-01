using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{
    /// <summary>
    /// [Trap 5 Fix] Service layer for Track — handles single-entity "Steps" only.
    /// No cross-entity orchestration here; that belongs to the Mediator.
    /// </summary>
    public interface ITrackService
    {
        Task<IEnumerable<TrackDto>> GetAllAsync();
        Task<TrackDto?> GetByIdAsync(int id);
        Task<TrackDto> CreateAsync(TrackDto dto);
        Task<bool> UpdateAsync(int id, TrackDto dto);
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Business step: checks if the track has room for another enrollment.
        /// Used by the EnrollInternMediator before creating an enrollment.
        /// </summary>
        Task<bool> CheckCapacityAsync(int trackId);
    }
}
