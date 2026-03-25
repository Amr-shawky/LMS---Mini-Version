using Microsoft.AspNetCore.Mvc;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;
using MediatR;

using LMS___Mini_Version.ViewModels.InternViewModels;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.CQRS.Interns.Query;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<internVM>>> GetAll()
        {
            var interns=await _mediator.Send(new GetAllInternsQuery());
            var internsViews = interns.Select(i => i.ToDo());
             return Ok(internsViews);
        }

        //[HttpGet("{id}")]
        //public ActionResult<Intern> GetById(int id)
        //{
        //    var intern = _context.Interns.Find(id);

        //    if (intern == null) return NotFound();

        //    return intern;
        //}

        //[HttpPost]
        //public ActionResult Create(Intern intern)
        //{
        //    _context.Interns.Add(intern);
        //    _context.SaveChanges();

        //    return Ok(intern);
        //}

        //[HttpPut("{id}")]
        //public ActionResult Update(int id, Intern updatedIntern)
        //{
        //    var intern = _context.Interns.Find(id);
        //    if (intern == null) return NotFound();

        //    intern.FullName = updatedIntern.FullName;
        //    intern.Email = updatedIntern.Email;
        //    intern.BirthYear = updatedIntern.BirthYear;
        //    intern.Status = updatedIntern.Status;
        //    intern.TrackId = updatedIntern.TrackId;

        //    _context.SaveChanges();
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
           

        //    return NoContent();
        //}
    }
}