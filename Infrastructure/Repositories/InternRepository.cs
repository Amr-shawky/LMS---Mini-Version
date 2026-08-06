namespace LMS___Mini_Version.Infrastructure.Repositories;

public class InternRepository : GeneralRepository<Intern> , IInternRepository
{
    private readonly AppDbContext _context;
    public InternRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<bool> IsEmailAddressTakenAsync(string email)
    {
        return await _context.Interns
            .AnyAsync(e => e.Email.ToLower() == email.ToLower());
        
    }

    
    public async Task<Intern?> GetWithTrackAsync(int InternId)
    {
        return await _context.Interns
            .Include(i => i.Track)
            .FirstOrDefaultAsync(i => i.Id == InternId);
    }

    public async Task<IEnumerable<Intern>> GetAllWithTrackAsync()
    {
        return await _context.Interns
            .Include(i => i.Track)
            .ToListAsync();
    }

    public async Task<IEnumerable<Intern>> GetAllByTrackIdAsync(int trackId)
    {
        return await _context.Interns
            .Include(e => e.Track)
            .Where(e => e.TrackId == trackId)
            .OrderBy(e => e.FullName)
            .ToListAsync();
    }
}