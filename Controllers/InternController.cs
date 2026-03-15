using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [CQRS Fix] Injects ONLY IMediator — no more IInternService.
    /// All responses wrapped in EndpointResponse for a consistent API contract.
    /// </summary>
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());
            return Ok(EndpointResponse<IEnumerable<InternDto>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<InternDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetInternByIdQuery(id));
            if (result == null)
                return NotFound(EndpointResponse<InternDto>.NotFoundResponse($"Intern with ID {id} was not found."));

            return Ok(EndpointResponse<InternDto>.SuccessResponse(result));
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<InternSummaryViewModel>>> Create(CreateInternViewModel vm)
        {
            var result = await _mediator.Send(new CreateInternCommand(
                vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId
            ));

            return Ok(EndpointResponse<InternSummaryViewModel>.SuccessResponse(result, "Intern created successfully.", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndpointResponse<string>>> Update(int id, UpdateInternViewModel vm)
        {
            var updated = await _mediator.Send(new UpdateInternCommand(
                id, vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId
            ));

            if (!updated)
                return NotFound(EndpointResponse<string>.NotFoundResponse($"Intern with ID {id} was not found."));

            return Ok(EndpointResponse<string>.SuccessResponse("Updated", "Intern updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse<string>>> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteInternCommand(id));
            if (!deleted)
                return NotFound(EndpointResponse<string>.NotFoundResponse($"Intern with ID {id} was not found."));

            return Ok(EndpointResponse<string>.SuccessResponse("Deleted", "Intern deleted successfully."));
        }
    }
}