
using LMS___Mini_Version.features.InternFeatures.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.ApproveInternRegistration.Command;
using LMS___Mini_Version.features.InternFeatures.DeleteIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Query;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.ViewModels;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;



        [HttpGet]
      public async Task<ActionResult<EndpointResponse<IEnumerable<InternsViewModel>>>> GetAllInterns()
        {
            var result = await mediator.Send(new GetAllInternsQuery());
            var Response = EndpointResponse.OK(200,
                result.Success ? "Interns retrieved successfully" : "Failed to retrieve interns"
            );
            var internsVM = result.Data?.Select(x => new InternsViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName= x.TrackName
            });
            return StatusCode(result.StatusCode,EndpointResponse<IEnumerable<InternsViewModel>>.OK(internsVM,result.StatusCode,"Success"));
        }   

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> CreateIntern(AddInternCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to create intern",
                        result.Errors));
            }

            return StatusCode(
                201,
                EndpointResponse.OK(201, "Intern created successfully"));
        }


        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}

        [HttpPost("{id}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<int>>> ApproveIntern(int id, [FromBody] ApproveAndRegisterInternCommand command)
        {
            if (id != command.InternId)
            {
                return BadRequest(EndpointResponse.Fail(400, "Intern ID in the URL does not match the request body"));
            }
            var result = await mediator.Send(command);
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse<int>.Fail(
                        result.StatusCode,
                        "Failed to approve intern",
                        result.Errors));
            }
            return StatusCode(
                result.StatusCode,
                EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern Registration approved successfully"));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id)
        {
            var result = await mediator.Send(new DeleteInternCommand(id));
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to delete intern",
                        result.Errors));
            } 
            return StatusCode(
                result.StatusCode,
                EndpointResponse.OK(result.StatusCode, "Intern deleted successfully"));
        }
    }
}
