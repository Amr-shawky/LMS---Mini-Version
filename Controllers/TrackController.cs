using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Track;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;

        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDetailViewModel>> GetById(int id)
        {
            var dto = await _trackService.GetByIdAsync(id);
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

            var created = await _trackService.CreateAsync(dto);

            return Ok(new TrackSummaryViewModel
            {
                Id = created.Id,
                Name = created.Name,
                Fees = created.Fees,
                IsActive = created.IsActive
            });
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
