using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InternController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 2: GetAllInternsQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Interns/Queries/
            // 2) Create the Handler class in Features/Interns/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
          var Result = await _mediator.Send(new GetAllInternsQuery());
            return Ok(Result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 3: GetInternByIdQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Interns/Queries/
            // 2) Create the Handler class in Features/Interns/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            var result = await _mediator.Send(new GetInternByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm)
        {
            var result = await _mediator.Send(new CreateInternCommand(
                vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId
            ));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateInternViewModel vm)
        {
            var updated = await _mediator.Send(new UpdateInternCommand(
                id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId
            ));

            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteInternCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}