using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Orchestrators;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [CQRS Fix] This controller injects ONLY IMediator.
    /// Read operations dispatch Queries.
    /// Write operations dispatch Orchestrator Requests (which coordinate atomic steps internally).
    /// All responses wrapped in EndpointResponse for a consistent API contract.
    /// </summary>
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllEnrollmentsQuery());
            return Ok(EndpointResponse<IEnumerable<EnrollmentDto>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<EnrollmentDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetEnrollmentByIdQuery(id));
            if (result == null)
                return NotFound(EndpointResponse<EnrollmentDto>.NotFoundResponse($"Enrollment with ID {id} was not found."));

            return Ok(EndpointResponse<EnrollmentDto>.SuccessResponse(result));
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentDto>>>> GetByIntern(int internId)
        {
            var result = await _mediator.Send(new GetEnrollmentsByInternQuery(internId));
            return Ok(EndpointResponse<IEnumerable<EnrollmentDto>>.SuccessResponse(result));
        }

        // ═══════════════════════════════════════════════════════
        //  ACTION ENDPOINTS (dispatched as Orchestrator Requests)
        // ═══════════════════════════════════════════════════════

        /// <summary>
        /// Enrolls an intern in a track.
        /// The EnrollInternOrchestratorHandler coordinates all steps internally.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EndpointResponse<EnrollmentWithPaymentDto>>> Enroll(EnrollInternViewModel vm)
        {
            var result = await _mediator
                .Send(new EnrollInternOrchestratorRequest(vm.InternId, vm.TrackId));

            if (!result.IsSuccess)
            {
                return BadRequest(EndpointResponse<EnrollmentWithPaymentDto>.ErrorResponse(result.Message));
            }

            return Ok(EndpointResponse<EnrollmentWithPaymentDto>.SuccessResponse(result.Data!, result.Message, 201));
        }

        /// <summary>
        /// Cancels an enrollment and refunds the payment.
        /// The CancelEnrollmentOrchestratorHandler coordinates all steps internally.
        /// </summary>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<EndpointResponse<string>>> Cancel(int id)
        {
            var result = await _mediator
                .Send(new CancelEnrollmentOrchestratorRequest(id));

            if (!result.IsSuccess)
            {
                return BadRequest(EndpointResponse<string>.ErrorResponse(result.Message));
            }

            return Ok(EndpointResponse<string>.SuccessResponse(result.Data!, result.Message));
        }

        /// <summary>
        /// Transfers an enrollment to a different track.
        /// The TransferEnrollmentOrchestratorHandler coordinates all steps internally.
        /// </summary>
        [HttpPost("{id}/transfer/{newTrackId}")]
        public async Task<ActionResult<EndpointResponse<string>>> Transfer(int id, int newTrackId)
        {
            var result = await _mediator
                .Send(new TransferEnrollmentOrchestratorRequest(id, newTrackId));

            if (!result.IsSuccess)
            {
                return BadRequest(EndpointResponse<string>.ErrorResponse(result.Message));
            }

            return Ok(EndpointResponse<string>.SuccessResponse(result.Data!, result.Message));
        }
    }
}
