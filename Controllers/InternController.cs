using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.InternFeature.CreateIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.DeleteUnusedIntern.Commands;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Queries;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.ViewModels;
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

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetAllInternsOutputViewModel>>>> GetAll(CancellationToken cancellationToken)
        {
            var interns = await _mediator.Send(new GetAllInternsQuery(), cancellationToken);
            if(!interns.Success)
            {
                return StatusCode(interns.StatusCode,
                    EndpointResponse<IEnumerable<GetAllInternsOutputViewModel>>.Fail(interns.StatusCode, "Failed to get interns", interns.Errors));
            }
            var internsVM = interns.Data.Select(x => new GetAllInternsOutputViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                TrackName = x.TrackName,
                Status = x.Status
            });

            var successResponse = EndpointResponse<IEnumerable<GetAllInternsOutputViewModel>>.OK(internsVM, interns.StatusCode, "success");
            return StatusCode(interns.StatusCode, successResponse);
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<int>>> Create(CreateInternInputViewModel vm, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(vm.FullName))
            {
                errors.Add("FullName", new[] { "Full name is required" });
            }
            if (string.IsNullOrWhiteSpace(vm.Email))
            {
                errors.Add("Email", new[] { "Email is required" });
            }
            if (vm.BirthYear > DateTime.UtcNow.Year || vm.BirthYear < 1900)
            {
                errors.Add("BirthYear", new[] { "Invalid birth year" });
            }
            if (vm.TrackId <= 0)
            {
                errors.Add("TrackId", new[] { "Track Id must be greater than zero" });
            }
            if (errors.Count > 0)
            {
                var errorResponse = EndpointResponse<int>.Fail(400, errors: errors);
                return BadRequest(errorResponse);
            }

            var command = new CreateInternCommand(vm.FullName, vm.Email, vm.BirthYear, vm.TrackId);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success)
            {
                var errorResponse = EndpointResponse<int>.Fail(result.StatusCode, errors: result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }

            var successResponse = EndpointResponse<int>.OK(result.Data, 201, "Intern created successfully");
            return StatusCode(201, successResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                var errorResponse = EndpointResponse.Fail(400, errors: new Dictionary<string, string[]> { { "InternId", new[] { "Intern ID must be greater than zero" } } });
                return BadRequest(errorResponse);
            }

            var result = await _mediator.Send(new DeleteUnusedInternCommand(id), cancellationToken);
            if (!result.Success)
            {
                var errorResponse = EndpointResponse.Fail(result.StatusCode, errors: result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }

            var successResponse = EndpointResponse.OK(200);
            return StatusCode(200, successResponse);
        }

        [HttpPost("{internId}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<int>>> ApproveAndRegister(int internId, [FromBody] ApproveAndRegisterInternInputViewModel vm, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (internId <= 0)
            {
                errors.Add("InternId", new[] { "Intern ID must be greater than zero." });
            }
            if (!Enum.IsDefined(typeof(PaymentMethod), vm.Method))
            {
                errors.Add("PaymentMethod", new[] { "Payment method is invalid." });
            }
            if (errors.Count > 0)
            {
                var errorResponse = EndpointResponse<int>.Fail(400, errors: errors);
                return BadRequest(errorResponse);
            }

            var command = new ApproveAndRegisterInternCommand(internId, vm.Method);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success)
            {
                var errorResponse = EndpointResponse<int>.Fail(result.StatusCode, errors: result.Errors);

                return StatusCode(result.StatusCode, errorResponse);
            }

            var successResponse = EndpointResponse<int>.OK(result.Data, 200, "Intern approved and registered successfully.");
            return Ok(successResponse);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}
    }
}
