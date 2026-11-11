
using LMS___Mini_Version.features.InternFeature.ApproveAndRegister.Commands;
using LMS___Mini_Version.features.InternFeature.ApproveAndRegister.ViewModel;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.InternFeature.CreateIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Commands;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
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
        private readonly IMediator mediator;

        public InternController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternViewModel>>>> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetAllInternsQuery(),
                cancellationToken);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to get interns",
                        result.Errors
                    )
                );
            }

            var internsViewModel = result.Data!
                .Select(x => new InternViewModel
                {
                    id = x.id,
                    fullName = x.fullName,
                    email = x.email,
                    status = x.status,
                    trackName = x.trackName
                })
                .ToList();

            return StatusCode(
                result.StatusCode,
                EndpointResponse<IEnumerable<InternViewModel>>.OK(
                    internsViewModel,
                    result.StatusCode,
                    "Interns retrieved successfully"
                )
            );
        }




        [HttpPost("{internId}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<int>>> ApproveAndRegister(
            int internId,
            [FromBody] ApproveAndRegisterViewModel model,
            CancellationToken cancellationToken)
        {
            if (internId <= 0)
            {
                return BadRequest(EndpointResponse.Fail(
                    400,
                    errors: new Dictionary<string, string[]>
                    {
                        ["InternId"] = new[]
                        {
                    "InternId must be greater than zero."
                        }
                    }));
            }

            var result = await mediator.Send(
                new ApproveAndRegisterCommand(internId, model.Method),
                cancellationToken);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse<int>.Fail(
                        result.StatusCode,
                        "Failed to approve and register intern",
                        result.Errors));
            }

            return StatusCode(
                result.StatusCode,
                EndpointResponse<int>.OK(
                    result.Data,
                    result.StatusCode,
                    "Intern approved and registered successfully"));
        }



        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<int>>> Create(
            CreateInternViewModel model)
        {
            var command = new CreateInternCommand(
                model.FullName,
                model.Email,
                model.BirthYear,
                model.TrackId
            );

            var result = await mediator.Send(command);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse<int>.Fail(
                        result.StatusCode,
                        "Failed to register intern",
                        result.Errors
                    )
                );
            }

            return StatusCode(
                result.StatusCode,
                EndpointResponse<int>.OK(
                    result.Data,
                    result.StatusCode,
                    "Intern registered successfully"
                )
            );
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(
                    EndpointResponse.Fail(
                        400,
                        errors: new Dictionary<string, string[]>
                        {
                            ["InternId"] = new[]
                            {
                        "InternId must be greater than zero."
                            }
                        }));
            }

            var result = await mediator.Send(
                new DeleteInternCommand(id),
                cancellationToken);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to delete intern",
                        result.Errors));
            }

            return Ok(
                EndpointResponse.OK(
                    200,
                    "Intern deleted successfully"));
        }

    }
}
