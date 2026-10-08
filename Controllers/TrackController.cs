using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackSummaryViewModel>>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllTracksQuery());
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<TrackSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data!.Select(d => d.ToSummaryViewModel());
            return Ok(EndpointResponse<IEnumerable<TrackSummaryViewModel>>.Ok(viewModels));
        }

        [HttpGet("cqrs")]
        public async Task<ActionResult<EndpointResponse<PaginatedResult<TrackSummaryViewModel>>>> GetAllCQRS([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllTrackQuery(pageIndex, pageSize));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse<PaginatedResult<TrackSummaryViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data!.Items.Select(d => d.ToSummaryViewModel()).ToList();
            var paginated = PaginatedResult<TrackSummaryViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
            return Ok(EndpointResponse<PaginatedResult<TrackSummaryViewModel>>.Ok(paginated));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<TrackDetailViewModel>>> GetByIdCQRS(int id)
        {
            var response = await _mediator.Send(new GetByIdTrackQuery(id));
            if (!response.Success || response.Data == null)
            {
                return NotFound(EndpointResponse<TrackDetailViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse<TrackDetailViewModel>.Ok(response.Data.ToDetailViewModel()));
        }

        [HttpPost("cqrs")]
        public async Task<ActionResult<EndpointResponse<TrackSummaryViewModel>>> CreateCQRS(CreateTrackViewModel vm)
        {
            var createResponse = await _mediator.Send(new CreateTrackCommand(vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
            if (!createResponse.Success)
            {
                return StatusCode(createResponse.StatusCode, EndpointResponse<TrackSummaryViewModel>.Fail(createResponse.Message, createResponse.StatusCode, createResponse.Errors));
            }

            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(createResponse.Data));
            var summaryVm = trackResponse.Data != null
                ? trackResponse.Data.ToSummaryViewModel()
                : new TrackSummaryViewModel { Id = createResponse.Data, Name = vm.Name, Fees = vm.Fees, IsActive = vm.IsActive };

            return Ok(EndpointResponse<TrackSummaryViewModel>.Created(summaryVm, "Track created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse>> Update(int id, UpdateTrackViewModel vm)
        {
            var response = await _mediator.Send(new UpdateTrackCommand(id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse.Ok("Track updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteTrackCommand(id));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse.Ok("Track deleted successfully"));
        }
    }
}