using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{

    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto?> GetByIdAsync(int id);

        Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentDto dto);

        Task<IEnumerable<EnrollmentDto>> GetByInternAsync(int internId);

        Task<bool> UpdateStatusAsync(int enrollmentId, EnrollmentStatus newStatus);

        Task<bool> UpdateTrackAsync(int enrollmentId, int newTrackId);
    }
}
