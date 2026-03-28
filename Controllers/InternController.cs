namespace LMS___Mini_Version.Controllers
{
     
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternController(IMediator mediator) => _mediator = mediator;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInternByIdQuery(id));
            return result is null ? NotFound($"Intern with Id {id} was not found.") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInternCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id ,[FromBody]UpdateInternCommand command)
        {
            var result = await _mediator.Send(command with { InternId = id });
            return Ok(new { id = result });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteInternCommand(id));
            return result ? NoContent() : NotFound($"Intern with Id {id} was not found.");
        }


    }
}