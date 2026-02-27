using Microsoft.AspNetCore.Mvc;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InternController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Intern> GetAll()
        {
            return _context.Interns.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Intern> GetById(int id)
        {
            var intern = _context.Interns.Find(id);

            if (intern == null) return NotFound();

            return intern;
        }

        [HttpPost]
        public ActionResult Create(Intern intern)
        {
            _context.Interns.Add(intern);
            _context.SaveChanges();

            return Ok(intern);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Intern updatedIntern)
        {
            var intern = _context.Interns.Find(id);
            if (intern == null) return NotFound();

            intern.FullName = updatedIntern.FullName;
            intern.Email = updatedIntern.Email;
            intern.BirthYear = updatedIntern.BirthYear;
            intern.Status = updatedIntern.Status;
            intern.TrackId = updatedIntern.TrackId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var intern = _context.Interns.Find(id);
            if (intern == null) return NotFound();

            _context.Interns.Remove(intern);
            _context.SaveChanges();

            return NoContent();
        }
    }
}