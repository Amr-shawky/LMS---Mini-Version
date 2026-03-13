using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{
    /// <summary>
    /// [Trap 5 Fix] Service layer for Intern — single-entity Steps only.
    /// </summary>
    public interface IInternService
    {
        Task<IEnumerable<InternDto>> GetAllAsync();
        Task<InternDto?> GetByIdAsync(int id);
        Task<InternDto> CreateAsync(InternDto dto);
        Task<bool> UpdateAsync(int id, InternDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
