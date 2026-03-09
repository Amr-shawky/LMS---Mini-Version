using LMS___Mini_Version.Features.Intern.Queries.GetAllInterns;
using LMS___Mini_Version.Features.Intern.Queries.GetInternById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Features.Intern
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternCQRSController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternCQRSController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInterByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);

        }
    }
}
