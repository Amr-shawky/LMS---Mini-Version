using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [Trap 1 Fix] This controller depends only on ITrackService (abstraction).
    /// [SRP Fix] No longer injects IUnitOfWork — the Service owns its own CRUD transactions.
    /// [Trap 2 Fix] All responses use ViewModels; all inputs use ViewModels.
    /// [Trap 3 Fix] Every action is async Task — no synchronous blocking.
    /// [Trap 5 Fix] No business logic in the controller — all delegated to TrackService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;
        IMediator _mediator;

        public TrackController(ITrackService trackService, IMediator mediator)
        {
            _trackService = trackService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackSummaryViewModel>>> GetAll()
        {
            var dtos = await _trackService.GetAllAsync();
            var viewModels = dtos.Select(d => d.ToSummaryViewModel());
            return Ok(viewModels);
        }
        [HttpGet("cqrs")]
        public async Task<ActionResult<IEnumerable<TrackSummaryViewModel>>> GetAllCQRS()
        {
            var dtos = await _mediator.Send(new GetAllTrackQuery());
            var viewModels = dtos.Select(d => d.ToSummaryViewModel());
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDetailViewModel>> GetByIdCQRS(int id)
        {
            var dto = await _mediator.Send(new GetByIdTrackQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto.ToDetailViewModel());
        }

        [HttpPost("cqrs")]
        public async Task<ActionResult<TrackSummaryViewModel>> CreateCQRS(CreateTrackViewModel vm)
        {


            var created = await _mediator.Send(new CreateTrackCommand(vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
            // No CompleteAsync here — the Service saves and returns DTO with correct Id
            return Ok("track created successfully");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTrackViewModel vm)
        {
            var dto = new TrackDto
            {
                Name = vm.Name,
                Fees = vm.Fees,
                IsActive = vm.IsActive,
                MaxCapacity = vm.MaxCapacity
            };

            var updated = await _trackService.UpdateAsync(id, dto).ConfigureAwait(false);
            if (!updated) return NotFound();

            // No CompleteAsync here — the Service saves internally
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _trackService.DeleteAsync(id).ConfigureAwait(false);
            if (!deleted) return NotFound();

            // No CompleteAsync here — the Service saves internally
            return NoContent();
        }
    }
}