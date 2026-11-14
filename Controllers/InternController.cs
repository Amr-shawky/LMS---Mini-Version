
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeature.GetAll_Interns.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddInternCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInterns()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());

            return Ok(result);
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        //[HttpPost]
        //public async Task<ActionResult<>> Create( )
        //{



        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete()
        //{

        //}
    }
}
