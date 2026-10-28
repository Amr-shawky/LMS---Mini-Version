using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Enrollment;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {

        private readonly IEnrollmentService _enrollmentService;
        private readonly EnrollInternMediator _enrollMediator;
        private readonly CancelEnrollmentMediator _cancelMediator;
        private readonly TransferEnrollmentMediator _transferMediator;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            EnrollInternMediator enrollMediator,
            CancelEnrollmentMediator cancelMediator,
            TransferEnrollmentMediator transferMediator
            )
        {
            _enrollmentService = enrollmentService;
            _enrollMediator = enrollMediator;
            _cancelMediator = cancelMediator;
            _transferMediator = transferMediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetAll()
        {
            var dtos = await _enrollmentService.GetAllAsync().ConfigureAwait(false);
            var viewModels = dtos.Select(d => new EnrollmentViewModel
            {
                Id = d.Id,
                InternName = d.InternName,
                TrackName = d.TrackName,
                EnrollmentDate = d.EnrollmentDate,
                Status = d.Status.ToString()
            });
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentViewModel>> GetById(int id)
        {
            var dto = await _enrollmentService.GetByIdAsync(id).ConfigureAwait(false);
            if (dto == null) return NotFound();
            return Ok(new EnrollmentViewModel
            {
                Id = dto.Id,
                InternName = dto.InternName,
                TrackName = dto.TrackName,
                EnrollmentDate = dto.EnrollmentDate,
                Status = dto.Status.ToString()
            });
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetByIntern(int internId)
        {
            var dtos = await _enrollmentService.GetByInternAsync(internId).ConfigureAwait(false);
            var viewModels = dtos.Select(d => new EnrollmentViewModel
            {
                Id = d.Id,
                InternName = d.InternName,
                TrackName = d.TrackName,
                EnrollmentDate = d.EnrollmentDate,
                Status = d.Status.ToString()
            });
            return Ok(viewModels);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
        {
            var result = await _enrollMediator.ExecuteAsync(new CreateEnrollmentDto
            {
                InternId = vm.InternId,
                TrackId = vm.TrackId
            }).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            var created = result.Enrollment!;
            return Ok(new EnrollmentViewModel
            {
                Id = created.Id,
                InternName = created.InternName,
                TrackName = created.TrackName,
                EnrollmentDate = created.EnrollmentDate,
                Status = created.Status.ToString()
            });
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> Cancel(int id)
        {
            var result = await _cancelMediator.ExecuteAsync(id).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("{id}/transfer/{newTrackId}")]
        public async Task<ActionResult> Transfer(int id, int newTrackId)
        {
            var result = await _transferMediator.ExecuteAsync(id, newTrackId).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
