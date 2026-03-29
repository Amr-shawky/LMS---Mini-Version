using LMS___Mini_Version.CQRS.Enrollments.Commands;
using LMS___Mini_Version.CQRS.Enrollments.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetAll()
        {
            var dtos = await _mediator.Send(new GetAllEnrollmentsQuery());
            var viewModels = dtos.Select(d => d.ToViewModel());
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentViewModel>> GetById(int id)
        {
            var dto = await _mediator.Send(new GetEnrollmentByIdQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto.ToViewModel());
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetByIntern(int internId)
        {
            var dtos = await _mediator.Send(new GetEnrollmentsByInternQuery(internId));
            var viewModels = dtos.Select(d => d.ToViewModel());
            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
        {
            var result = await _mediator.Send(new CreateEnrollmentCommand(vm.InternId, vm.TrackId));

            if (!result.IsSuccess)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(result.Enrollment!.ToViewModel());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            await _mediator.Send(new UpdateEnrollmentCommand(id, status));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteEnrollmentCommand(id));
            return NoContent();
        }
    }
}
