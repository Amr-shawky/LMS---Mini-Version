using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
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
        public async Task<ActionResult<EndpointResponse<PaginatedResult<EnrollmentViewModel>>>> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllEnrollmentsQuery(pageIndex, pageSize));
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.StatusCode, EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data.Items.Select(d => d.ToViewModel()).ToList();
            var paginated = PaginatedResult<EnrollmentViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
            return Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(paginated));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<EnrollmentViewModel>>> GetById(int id)
        {
            var response = await _mediator.Send(new GetEnrollmentByIdQuery(id));
            if (!response.Success || response.Data == null)
            {
                return NotFound(EndpointResponse<EnrollmentViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse<EnrollmentViewModel>.Ok(response.Data.ToViewModel()));
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<PaginatedResult<EnrollmentViewModel>>>> GetByIntern(int internId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetEnrollmentsByInternQuery(internId, pageIndex, pageSize));
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.StatusCode, EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data.Items.Select(d => d.ToViewModel()).ToList();
            var paginated = PaginatedResult<EnrollmentViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
            return Ok(EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(paginated));
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<EnrollmentViewModel>>> Enroll(EnrollInternViewModel vm)
        {
            var result = await _mediator.Send(new EnrollInternOrchestrator(vm.InternId, vm.TrackId));
            if (!result.Success || result.Data == null)
            {
                return StatusCode(result.StatusCode, EndpointResponse<EnrollmentViewModel>.Fail(result.Message, result.StatusCode, result.Errors));
            }

            return Ok(EndpointResponse<EnrollmentViewModel>.Created(result.Data.ToViewModel(), "Enrollment created successfully"));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<EndpointResponse>> Cancel(int id)
        {
            var result = await _mediator.Send(new cancelEnrollmentOrchestrator(id));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
            }

            return Ok(EndpointResponse.Ok("Enrollment cancelled successfully"));
        }

        [HttpPost("{id}/transfer/{newTrackId}")]
        public async Task<ActionResult<EndpointResponse>> Transfer(int id, int newTrackId)
        {
            var result = await _mediator.Send(new TransferEnrollmentOrchestrator(id, newTrackId));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse.Fail(result.Message, result.StatusCode, result.Errors));
            }

            return Ok(EndpointResponse.Ok("Enrollment transferred successfully"));
        }
    }
}
