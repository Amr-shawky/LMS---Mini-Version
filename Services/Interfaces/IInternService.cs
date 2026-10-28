using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{

    public interface IInternService
    {
        Task<IEnumerable<InternDto>> GetAllAsync();
        Task<InternDto?> GetByIdAsync(int id);
        Task<InternDto> CreateAsync(InternDto dto);
        Task<bool> UpdateAsync(int id, InternDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
