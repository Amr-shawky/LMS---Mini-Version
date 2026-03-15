using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.Tracks.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [CQRS Fix] This controller injects ONLY IMediator.
    /// All operations are dispatched as Commands (writes) or Queries (reads).
    /// All responses wrapped in EndpointResponse for a consistent API contract.
    /// </summary>
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTracksQuery());
            return Ok(EndpointResponse<IEnumerable<TrackDto>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<TrackDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetTrackByIdQuery(id));
            if (result == null)
                return NotFound(EndpointResponse<TrackDto>.NotFoundResponse($"Track with ID {id} was not found."));

            return Ok(EndpointResponse<TrackDto>.SuccessResponse(result));
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<TrackSummaryViewModel>>> Create(CreateTrackViewModel vm)
        {
            var result = await _mediator.Send(new CreateTrackCommand(
                vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity
            ));

            return Ok(EndpointResponse<TrackSummaryViewModel>.SuccessResponse(result, "Track created successfully.", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<string>>> Update(int id, UpdateTrackViewModel vm)
        {
            var updated = await _mediator.Send(new UpdateTrackCommand(
                id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity
            ));

            if (!updated)
                return NotFound(EndpointResponse<string>.NotFoundResponse($"Track with ID {id} was not found."));

            return Ok(EndpointResponse<string>.SuccessResponse("Updated", "Track updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse<string>>> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteTrackCommand(id));
            if (!deleted)
                return NotFound(EndpointResponse<string>.NotFoundResponse($"Track with ID {id} was not found."));

            return Ok(EndpointResponse<string>.SuccessResponse("Deleted", "Track deleted successfully."));
        }
    }
}