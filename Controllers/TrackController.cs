using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.Tracks.Commands;
using LMS___Mini_Version.Feature.Tracks.Query;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

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
            var viewModels = dtos.Select(d => new TrackSummaryViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Fees = d.Fees,
                IsActive = d.IsActive
            });
            return Ok(viewModels);
        }
        [HttpGet("cqrs")]
        public async Task<ActionResult<IEnumerable<TrackSummaryViewModel>>> GetAllCQRS()
        {
            var dtos = await _mediator.Send(new GetAllTrackQuery());
            var viewModels = dtos.Select(d => new TrackSummaryViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Fees = d.Fees,
                IsActive = d.IsActive
            });
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDetailViewModel>> GetByIdCQRS(int id)
        {
            var dto = await _mediator.Send(new GetByIdTrackQuery(id));
            if (dto == null) return NotFound();
            return Ok(new TrackDetailViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Fees = dto.Fees,
                IsActive = dto.IsActive,
                MaxCapacity = dto.MaxCapacity,
                CurrentEnrollmentCount = dto.CurrentEnrollmentCount
            });
        }

        [HttpPost("cqrs")]
        public async Task<ActionResult<TrackSummaryViewModel>> CreateCQRS(CreateTrackViewModel vm)
        {

            var created = await _mediator.Send(new CreateTrackCommand(vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));

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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _trackService.DeleteAsync(id).ConfigureAwait(false);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
