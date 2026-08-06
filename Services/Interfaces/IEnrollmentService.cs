using LMS___Mini_Version.DTOs;

namespace LMS___Mini_Version.Services.Interfaces
{
    /// <summary>
    /// [Trap 5 Fix] Service layer for Enrollment — single-entity Steps only.
    /// </summary>
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates the enrollment entity in memory (staged in Change Tracker).
        /// Does NOT call SaveChanges — that's the Mediator's job via UoW.
        /// </summary>
        Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto dto);

        Task<IEnumerable<EnrollmentDto>> GetByInternAsync(int internId);
    }
}
