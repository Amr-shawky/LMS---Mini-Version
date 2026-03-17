using LMS___Mini_Version.CQRS.Intern.Commands;
using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternController : ControllerBase
    {
        private readonly IMediator mediator;

        public InternController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetAll(int page, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllInternsQuery(cancellationToken, page));
            if (result.IsSuccess)
            {
                var viewModels = result.Data.Select(d => d.ToSummaryViewModel());
                return Ok(viewModels);
            }
            return NotFound(result);

        }
        [HttpGet("GetById")]
        public async Task<ActionResult<IEnumerable<InternSummaryViewModel>>> GetById(int Id,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetByIdInternsQuery(Id,cancellationToken));
            if (result.IsSuccess)
            {
                var viewModels = result.Data.ToSummaryViewModel();
                return Ok(viewModels);
            }
            return NotFound(result);

        }

        [HttpPost("Create")]
        public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var command = new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId, cancellationToken);

                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Message);
            }
            return BadRequest(vm);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<bool>> Update(int id, CreateInternViewModel vm, CancellationToken cancellationToken)
        {

            var command = new UpdateInternCommand(id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId, cancellationToken);

            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);

        }
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(int id,CancellationToken cancellationToken)
        {

            var command = new DeleteInternCommand(id,cancellationToken);

            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);

        }



    }


   
}

