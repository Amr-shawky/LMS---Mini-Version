using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Tracks.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        private readonly IMediator _mediator;
        public TrackController(ITrackService trackService, IMediator mediator)
        {
            _trackService = trackService;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackSummaryViewModel>>> GetAll()
        {
            var dtos = await _trackService.GetAllAsync().ConfigureAwait(false);
            var viewModels = dtos.Select(d => d.ToSummaryViewModel());
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        //GetById Track and Return TrackDetailViewModel by MediatorR
        public async Task<ActionResult<TrackDetailViewModel>> GetById(int id)
        {
            var dto = await _mediator.Send(new GetTrackByIdQuery(id));
            return dto == null ? NotFound() : Ok(dto.ToDetailViewModel());
        }

        [HttpPost]
        public async Task<ActionResult<TrackSummaryViewModel>> Create(CreateTrackViewModel vm)
        {
            var dto = new TrackDto
            {
                Name = vm.Name,
                Fees = vm.Fees,
                IsActive = vm.IsActive,
                MaxCapacity = vm.MaxCapacity
            };

            var created = await _trackService.CreateAsync(dto).ConfigureAwait(false);
            // No CompleteAsync here — the Service saves and returns DTO with correct Id
            return Ok(created.ToSummaryViewModel());
        }

        [HttpPut("{id}")]
        //update Track and Return NoContent by MediatorR
        public async Task<ActionResult> Update(int id, [FromBody] UpdateTrackViewModel vm)
        {
            var IsUpdate = await _mediator.Send(new UpdateTrackCommand(id, vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity));
            return IsUpdate ? NoContent() : NotFound($"This Track With ID {id} Not Found");
        }

        [HttpDelete("{id}")]
        //Delete Track and Return NoContent by MediatorR
        public async Task<ActionResult> Delete(int id)
        {
            var IsDeleted = await _mediator.Send(new DeleteTrackCommand(id));
            return IsDeleted ? NoContent() : NotFound($"This Track With ID {id} Not Found");
        }
    }
}