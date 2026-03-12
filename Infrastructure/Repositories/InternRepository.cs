using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Persistence;
using Microsoft.EntityFrameworkCore;

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
            .AnyAsync(e=>e.Email.ToLower() == email.ToLower())
            .ConfigureAwait(false);
        
    }

    public async Task<Intern?> GetWithTrackAsync(int id)
    {
        return await _context.Interns
            .Include(i => i.Track)
            .FirstOrDefaultAsync(i => i.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Intern>> GetAllWithTrackAsync()
    {
        return await _context.Interns
            .Include(i => i.Track)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<Intern>> GetByTrackAsync(int trackId)
    {
        return await _context.Interns
            .Include(e=>e.Track)
            .Where(e=>e.TrackId == trackId)
            .OrderBy(e=>e.FullName)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}