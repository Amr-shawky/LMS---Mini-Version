using Microsoft.AspNetCore.Mvc;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Track> GetAll()
        {
            var tracks = _context.Tracks.ToList();
            return tracks;
        }

        [HttpGet("{id}")]
        public ActionResult<Track> GetById(int id)
        {
            var track = _context.Tracks.Find(id);

            if (track == null)
            {
                return NotFound();
            }

            return track;
        }

        [HttpPost]
        public ActionResult Create(Track track)
        {
            _context.Tracks.Add(track);
            _context.SaveChanges();

            return Ok(track);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Track updatedTrack)
        {
            var track = _context.Tracks.Find(id);
            if (track == null) return NotFound();

            track.Name = updatedTrack.Name;
            track.Fees = updatedTrack.Fees;
            track.IsActive = updatedTrack.IsActive;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var track = _context.Tracks.Find(id);
            if (track == null) return NotFound();

            _context.Tracks.Remove(track);
            _context.SaveChanges();

            return NoContent();
        }
    }
}