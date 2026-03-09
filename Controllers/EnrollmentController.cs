using LMS___Mini_Version.Features.Enrollments.Orchestrators;
using LMS___Mini_Version.Features.Enrollments.Queries;
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

        // ═══════════════════════════════════════════════════════
        //  READ ENDPOINTS (dispatched as Queries)
        // ═══════════════════════════════════════════════════════

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetAll()
        {
            var result = await _mediator
                .Send(new GetAllEnrollmentsQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentViewModel>> GetById(int id)
        {
            var result = await _mediator
                .Send(new GetEnrollmentByIdQuery(id));

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult> GetByIntern(int internId)
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 5: GetEnrollmentsByInternQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Enrollments/Queries/
            // 2) Create the Handler class in Features/Enrollments/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            throw new NotImplementedException("Task 5: Wire this endpoint using IMediator");
        }

        // ═══════════════════════════════════════════════════════
        //  ACTION ENDPOINTS (dispatched as Orchestrator Requests)
        // ═══════════════════════════════════════════════════════

        [HttpPost]
        public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
        {
            var result = await _mediator
                .Send(new EnrollInternOrchestratorRequest(vm.InternId, vm.TrackId));

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result.Enrollment!.ToViewModel());
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> Cancel(int id)
        {
            var result = await _mediator
                .Send(new CancelEnrollmentOrchestratorRequest(id));

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("{id}/transfer/{newTrackId}")]
        public async Task<ActionResult> Transfer(int id, int newTrackId)
        {
            var result = await _mediator
                .Send(new TransferEnrollmentOrchestratorRequest(id, newTrackId));

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
