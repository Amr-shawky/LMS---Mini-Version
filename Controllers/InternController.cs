using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Intern;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS___Mini_Version.CQRS.Interns.Commands;
using LMS___Mini_Version.CQRS.Interns;
using LMS___Mini_Version.CQRS.Interns.Queries;

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
    public class InternController(IMediator _mediator) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetAll()
        {
           var interns = await _mediator.Send(new GetAllInternsQuery());
            return Ok(interns);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InternDetailViewModel>> GetById([FromQuery]int id)
        {
         var intern = await _mediator.Send(new GetInternByIdQuery(id));
         return Ok(intern);
        }

        [HttpPost]
        public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm)
        {
          var createdVm = await _mediator.Send(new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.TrackId, vm.Status));
            return Ok(createdVm);
         }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id,[FromBody] UpdateInternViewModel vm)
        {
            var updated = await _mediator.Send(new UpdateInternCommand(id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId));
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            var deletedEntity = await _mediator.Send(new DeleteInternCommand(id));
            return Ok(deletedEntity);
        }
    }
}