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
        var track = await _context.Tracks
            .FirstOrDefaultAsync(t => t.Id == trackId);

        if (track == null) return false;

        var activeCount = await _context.Enrollments
            .CountAsync(e => e.TrackId == trackId 
                       && e.Status != EnrollmentStatus.Cancelled);
        
        return activeCount < track.MaxCapacity;
    }

    public async Task<IEnumerable<Track>> GetActiveTracksAsync()
    {
        return await _context.Tracks
            .Where(t => t.IsActive)
            .ToListAsync();
    }

    public async Task<Track?> GetByIdWithEnrollmentCountAsync(int id)
    {
        var track = await _context.Tracks
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if(track == null) return null;

        track.MaxCapacity = await _context.Enrollments
            .CountAsync(e => e.TrackId == track.Id
                       && e.Status != EnrollmentStatus.Cancelled);
        return track;
    }

    public async Task<bool> IsNameTakenAsync(string name)
    {
        return await _context.Tracks.AnyAsync(t => t.Name.ToLower() == name.ToLower());
        
    }

    public async Task<bool> IsTrackActiveAsync(int id)
    {
        return await _context.Tracks.AnyAsync(t => t.Id == id && t.IsActive);
    }
}