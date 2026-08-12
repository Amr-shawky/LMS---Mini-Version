using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [Trap 1 Fix] Depends on IInternService — NOT AppDbContext.
    /// [SRP Fix] No longer injects IUnitOfWork — the Service owns its own CRUD transactions.
    /// [Trap 2 Fix] Accepts/returns ViewModels only.
    /// [Trap 3 Fix] Fully async.
    /// [Trap 5 Fix] Zero business logic — delegated to InternService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInternService _internService;

        public InternController(IMediator mediator, IInternService internService)
        {
            _mediator = mediator;
            _internService = internService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetAll()
        {
            //var dtos = await _internService.GetAllAsync().ConfigureAwait(false);
            var dtos=await _mediator.Send(new GetAllInternsQuery());
            var viewModels = dtos.Select(d => d.ToSummaryViewModel());
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InternDetailViewModel>> GetById(int id)
        {
            //var dto = await _internService.GetByIdAsync(id).ConfigureAwait(false);
            var dto = await _mediator.Send(new GetInternByIdQuery() { Id = id });

            if (dto == null) return NotFound();
            return Ok(dto.ToDetailViewModel());
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
            return Ok(created.ToSummaryViewModel());
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