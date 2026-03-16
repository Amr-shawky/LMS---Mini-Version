using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Commands.CreateInternCommand;
using LMS___Mini_Version.Features.Interns.Commands.DeleteInternCommand;
using LMS___Mini_Version.Features.Interns.Commands.UpdateInternCommand;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Interns.Queries.GetAllInternQuery;
using LMS___Mini_Version.Features.Interns.Queries.GetInternByIdQuery;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
        /// <summary>
        /// Implement SRP & Clean Architecture.
        ///  Intern controller does
        /// Receive requests from the client,
        ///  Delegate to Mediator,
        ///  Return responses to the client.
        /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InternController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetInternByIdQuery(id));
            return result is null ? NotFound($"Intern with Id {id} was not found.") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInternCommand command)
        {
            // Handler returns new Id
            var id = await _mediator.Send(command);
            // 201 Created 
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id ,[FromBody]UpdateInternCommand command)
        {
            var result = await _mediator.Send(command with { InternId = id });
            return Ok(new { id = result });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteInternCommand(id));
            return result ? NoContent() : NotFound($"Intern with Id {id} was not found.");
        }
    }
}