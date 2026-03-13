using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Intern;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.Intern.Commands;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [Trap 1 Fix] Depends on IInternService + IUnitOfWork — NOT AppDbContext.
    /// [Trap 2 Fix] Accepts/returns ViewModels only.
    /// [Trap 3 Fix] Fully async.
    /// [Trap 5 Fix] Zero business logic — delegated to InternService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        //private readonly IInternService _internService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public InternController(IInternService internService, IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            //_internService = internService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetAll()
        {
            //var dtos = await _internService.GetAllAsync().ConfigureAwait(false);

            /// Using MediatR to get all interns via a query handler
            /// instead of directly calling the service.
            var dtos = await _mediator.Send(new GetAllInternQuery()).ConfigureAwait(false);
            var viewModels = dtos.Select(d => d.ToSummaryViewModel());
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InternDetailViewModel>> GetById(int id)
        {
            //var dto = await _internService.GetByIdAsync(id).ConfigureAwait(false);

            /// refactor to use MediatR to get an intern by ID via a query handler
            /// instead of directly calling the service.
            /// Using MediatR to get an intern by ID via a query handler           

            var dto = await _mediator.Send(new GetInternByIdQuery(id)).ConfigureAwait(false);
            if (dto == null) return NotFound();
            return Ok(dto.ToDetailViewModel());
        }

        [HttpPost]
        public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm)
        {
            //var dto = new InternDto
            //{
            //    FullName = vm.FullName,
            //    Email = vm.Email,
            //    BirthYear = vm.BirthYear,
            //    Status = vm.Status,
            //    TrackId = vm.TrackId
            //};

            //var created = await _internService.CreateAsync(dto).ConfigureAwait(false);

            var dto = await _mediator.Send(new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId))
                .ConfigureAwait(false);
            await _unitOfWork.CompleteAsync().ConfigureAwait(false);

            return Ok(dto.ToSummaryViewModel());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateInternViewModel vm)
        {
            //var dto = new InternDto
            //{
            //    FullName = vm.FullName,
            //    Email = vm.Email,
            //    BirthYear = vm.BirthYear,
            //    Status = vm.Status,
            //    TrackId = vm.TrackId
            //};

            //var updated = await _internService.UpdateAsync(id, dto).ConfigureAwait(false);
            //if (!updated) return NotFound();

            var updated = await _mediator.Send(new UpdateInternCommand(id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId))
                .ConfigureAwait(false);

            await _unitOfWork.CompleteAsync().ConfigureAwait(false);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var deleted = await _internService.DeleteAsync(id).ConfigureAwait(false);

            var deleted = await _mediator.Send(new DeleteInternCommand(id)).ConfigureAwait(false);
            if (!deleted) return NotFound();
            await _unitOfWork.CompleteAsync().ConfigureAwait(false);
            return NoContent();
        }
    }
}