using LMS___Mini_Version.Features.Tracks.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
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
        public async Task<ActionResult<IEnumerable<TrackSummaryViewModel>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTracksQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 1: GetTrackByIdQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Tracks/Queries/
            //    (NOTE: GetTrackByIdQuery.cs already exists — create the Handler)
            // 2) Create the Handler class in Features/Tracks/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            var track = await _mediator.Send(new GetTrackByIdQuery(id));
            if (track == null) return NotFound();  

            return Ok(track);
        }

        [HttpGet("active")]
        public async Task<ActionResult> GetActiveTracks()
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 4: GetActiveTracksQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Tracks/Queries/
            // 2) Create the Handler class in Features/Tracks/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            
            var activeTracks = await _mediator.Send(new GetActiveTracksQuery());
            return Ok(activeTracks);

        }

        [HttpPost]
        public async Task<ActionResult<TrackSummaryViewModel>> Create(CreateTrackViewModel vm)
        {
            var result = await _mediator.Send(new CreateTrackCommand(
                vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity
            ));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTrackViewModel vm)
        {
            var updated = await _mediator.Send(new UpdateTrackCommand(
                id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity
            ));
            if (!updated) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteTrackCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}