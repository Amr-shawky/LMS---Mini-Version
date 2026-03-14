using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Infrastructure.Repositories;

public class EnrollmentRepository : GeneralRepository<Enrollment> , IEnrollmentRepository
{
    private readonly AppDbContext _context;
    
    public EnrollmentRepository(AppDbContext context) : base(context)
    {
        _context = context;

    }

    public async Task<bool> HasActiveEnrollmentAsync(int internId,int trackId)
    {
        return await _context.Enrollments.
            AnyAsync(e=>e.InternId == internId && e.TrackId == trackId &&
                        e.Status == Domain.Enums.EnrollmentStatus.Active)
            .ConfigureAwait(false);
    }

    public async Task<int> GetActiveEnrollmentCountByTrackIdAsync(int trackId)
    {
        if(trackId <= 0)
            throw new Exception("TrackId must be greater than 0");
        return await _context.Enrollments
            .CountAsync(e => e.TrackId == trackId &&
                             e.Status == Domain.Enums.EnrollmentStatus.Active)
            .ConfigureAwait(false);
    }

    public async Task<Enrollment?> GetActiveEnrollmentByInternIdAsync(int internId)
    {
        return await _context.Enrollments.FirstOrDefaultAsync(e => e.InternId == internId).ConfigureAwait(false);
        
    }

    public async Task<IEnumerable<Enrollment>> GetByTrackIdWithDetailsAsync(int trackId)
    {
        return await _context.Enrollments
            .Include(e => e.Track)
            .Where(e => e.TrackId == trackId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetAllByInternIdAsync(int internId)
    {
        return await _context.Enrollments
            .Include(e => e.Intern)
            .Where(e=>e.TrackId == internId)
            .ToListAsync()
            .ConfigureAwait(false);
    }


    public async Task<bool> CancelEnrollmentAsync(int enrollmentId)
    {
        var enrollment = await _context.Enrollments.FirstOrDefaultAsync
            (e => e.Id == enrollmentId).ConfigureAwait(false);

        if (enrollment == null) return false;
        if (enrollment.Status == EnrollmentStatus.Cancelled) return false;
        
        enrollment.Status = EnrollmentStatus.Cancelled;
        return true;
    }
}