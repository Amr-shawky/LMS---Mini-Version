
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeature.AddIntern.Dtos;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Dtos;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Commands;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
using LMS___Mini_Version.features.shared;
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

        // Task 02 — Browse interns (all statuses, with their track name)
        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternListItemDto>>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllInternsQuery(), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get interns", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<IEnumerable<InternListItemDto>>.OK(result.Data!, result.StatusCode, "success"));
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        // Task 01 — Register a new intern (Status = Pending)
        [HttpPost]
        public async Task<ActionResult<EndpointResponse<CreatedInternDto>>> Create(
            [FromBody] AddInternCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Intern could not be registered", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<CreatedInternDto>.OK(result.Data!, result.StatusCode,
                    $"Intern registered with id {result.Data!.Id} and is awaiting review"));
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}

        // Task 05 — Remove an intern only if they never had an enrollment
        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteInternCommand(id), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Intern could not be deleted", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse.OK(result.StatusCode, $"Intern {id} deleted"));
        }

        // Task 08 — Approve a Pending intern and register them (enrollment + completed payment) in one action
        [HttpPost("{internId}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<InternRegistrationDto>>> ApproveAndRegister(
            int internId, [FromBody] ApproveAndRegisterInternRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ApproveAndRegisterInternCommand(internId, request.PaymentMethod), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Intern could not be approved and registered", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<InternRegistrationDto>.OK(result.Data!, result.StatusCode,
                    $"Intern approved and registered. Enrollment id {result.Data!.EnrollmentId}"));
        }
    }
}
