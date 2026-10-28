using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Intern;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IInternService _internService;

        public InternController(IInternService internService)
        {
            _internService = internService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetAll()
        {
            var dtos = await _internService.GetAllAsync().ConfigureAwait(false);
            var viewModels = dtos.Select(d => new InternSummaryViewModel
            {
                Id = d.Id,
                FullName = d.FullName,
                Email = d.Email,
                Status = d.Status,
                TrackName = d.TrackName
            });
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InternDetailViewModel>> GetById(int id)
        {
            var dto = await _internService.GetByIdAsync(id).ConfigureAwait(false);
            if (dto == null) return NotFound();
            return Ok(new InternDetailViewModel
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                BirthYear = dto.BirthYear,
                Status = dto.Status,
                TrackId = dto.TrackId,
                TrackName = dto.TrackName
            });
        }

        [HttpPost]
        public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm)
        {
            var dto = new InternDto
            {
                FullName = vm.FullName,
                Email = vm.Email,
                BirthYear = vm.BirthYear,
                Status = vm.Status,
                TrackId = vm.TrackId
            };

            var created = await _internService.CreateAsync(dto).ConfigureAwait(false);
            return Ok(new InternSummaryViewModel
            {
                Id = created.Id,
                FullName = created.FullName,
                Email = created.Email,
                Status = created.Status,
                TrackName = created.TrackName
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateInternViewModel vm)
        {
            var dto = new InternDto
            {
                FullName = vm.FullName,
                Email = vm.Email,
                BirthYear = vm.BirthYear,
                Status = vm.Status,
                TrackId = vm.TrackId
            };

            var updated = await _internService.UpdateAsync(id, dto).ConfigureAwait(false);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _internService.DeleteAsync(id).ConfigureAwait(false);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
