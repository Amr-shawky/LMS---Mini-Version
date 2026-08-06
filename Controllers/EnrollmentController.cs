using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {

        private readonly IMediator _mediator;
        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllEnrollmentsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetEnrollmentByIdQuery(id)).ConfigureAwait(false);
            return Ok(result.ToViewModel());
        }

        [HttpGet("intern/{internId}")]
        public async Task<IActionResult> GetByIntern(int internId)
        {
            var result = await _mediator.Send(new GetEnrollmentsByInternId(internId)).ConfigureAwait(false);
            return Ok(result);

        }

        [HttpGet("intern/{internId:int}/active")]
        public async Task<IActionResult> GetActiveByIntern(int internId)
        {
            var result = await _mediator.Send(new GetActiveEnrollmentByInternIdQuery(internId)).ConfigureAwait(false);
            return Ok(result?.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentInternCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new
                {
                    succeeded = false,
                    message = result.ErrorMessage
                });
            // return CreatedAtAction(nameof(GetById), new { id = result.Enrollment.Id },
            //     new { id = result.Enrollment.Id });
            return Ok(result.Enrollment.ToViewModel());
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _mediator.Send(new CancelEnrollmentCommand(id)).ConfigureAwait(false);
            if (!result.IsSuccess)
                return BadRequest(new
                {
                    succeeded = false,
                    message = result.ErrorMessage
                });
            return Ok(new
            {
                succeeded = true,
                enrollment = result.Enrollment,
                payment = result.Payment
            });
        }

    } 
}
