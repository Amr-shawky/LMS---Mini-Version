using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;

namespace LMS___Mini_Version.Domain.Repositories;

public interface IEnrollmentRepository :IGeneralRepository<Enrollment>
{
    Task<bool> HasActiveEnrollmentAsync(int internId);
    
    Task<int> GetActiveEnrollmentCountByTrackIdAsync(int trackId);
    Task<Enrollment?> GetEnrollmentByInternIdAsync(int internId);
    
    Task<IEnumerable<Enrollment>> GetByIdWithDetailsAsync(int trackId, EnrollmentStatus status);
    
    Task<bool> CancelEnrollmentAsync(int enrollmentId);
}