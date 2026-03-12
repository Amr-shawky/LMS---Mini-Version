using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Infrastructure.Repositories;

public class TrackRepository : GeneralRepository<Track> , ITrackRepository
{
    private readonly AppDbContext _context;
    public TrackRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckCapacityAsync(int trackId)
    {
        var track =await _context.Tracks
            .FirstOrDefaultAsync(t => t.Id == trackId)
            .ConfigureAwait(false);

        if (track == null) return false;
        
        var activeCount = await _context.Enrollments
            .CountAsync(e=>e.TrackId == trackId && e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
            .ConfigureAwait(false);
        
        return activeCount < track.MaxCapacity;
    }

    public async Task<IEnumerable<Track>> GetActiveTracksAsync()
    {
        return await _context.Tracks
            .Where(t => t.IsActive)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<Track?> GetByIdWithEnrollmentCountAsync(int id)
    {
        var track =await _context.Tracks
            .FirstOrDefaultAsync(t => t.Id == id)
            .ConfigureAwait(false);
        
        if(track == null) return null;
        
        track.MaxCapacity = await _context.Enrollments
            .CountAsync(e=>e.TrackId == track.Id && e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
            .ConfigureAwait(false);
        return track;
    }

    public async Task<bool> IsNameTakenAsync(string name)
    {
        return await _context.Tracks.AnyAsync(t => t.Name.ToLower() == name.ToLower())
            .ConfigureAwait(false);
        
    }
}