using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;

namespace LMS___Mini_Version.Domain.Repositories;

public interface IEnrollmentRepository :IGeneralRepository<Enrollment>
{
    Task<bool> HasActiveEnrollmentAsync(int internId,int trackId);
    
    Task<int> GetActiveEnrollmentCountByTrackIdAsync(int trackId);
    Task<Enrollment?> GetActiveEnrollmentByInternIdAsync(int internId);
    
    Task<IEnumerable<Enrollment>> GetAllByInternIdAsync(int internId);
    Task<IEnumerable<Enrollment>> GetByTrackIdWithDetailsAsync(int trackId);
    
    Task<bool> CancelEnrollmentAsync(int enrollmentId);
}