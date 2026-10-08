using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Commands;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<PaginatedResult<InternSummaryViewModel>>>> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllInternsQuery(pageIndex, pageSize));
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.StatusCode, EndpointResponse<PaginatedResult<InternSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data.Items.Select(d => d.ToSummaryViewModel()).ToList();
            var paginated = PaginatedResult<InternSummaryViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
            return Ok(EndpointResponse<PaginatedResult<InternSummaryViewModel>>.Ok(paginated));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<InternDetailViewModel>>> GetById(int id)
        {
            var response = await _mediator.Send(new GetInternByIdQuery(id));
            if (!response.Success || response.Data == null)
            {
                return NotFound(EndpointResponse<InternDetailViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse<InternDetailViewModel>.Ok(response.Data.ToDetailViewModel()));
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<InternSummaryViewModel>>> Create(CreateInternViewModel vm)
        {
            var createResponse = await _mediator.Send(new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId));
            if (!createResponse.Success)
            {
                return StatusCode(createResponse.StatusCode, EndpointResponse<InternSummaryViewModel>.Fail(createResponse.Message, createResponse.StatusCode, createResponse.Errors));
            }

            var internResponse = await _mediator.Send(new GetInternByIdQuery(createResponse.Data));
            var summaryVm = internResponse.Data != null
                ? internResponse.Data.ToSummaryViewModel()
                : new InternSummaryViewModel { Id = createResponse.Data, FullName = vm.FullName, Email = vm.Email, Status = vm.Status };

            return Ok(EndpointResponse<InternSummaryViewModel>.Created(summaryVm, "Intern created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse>> Update(int id, UpdateInternViewModel vm)
        {
            var response = await _mediator.Send(new UpdateInternCommand(id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse.Ok("Intern updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteInternCommand(id));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse.Ok("Intern deleted successfully"));
        }
    }
}