
using LMS___Mini_Version.features.InternFeature.AddIntern.Command;
using LMS___Mini_Version.features.InternFeature.BrowseInterns.Query;
using LMS___Mini_Version.features.InternFeature.BrowseInterns.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
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
        public async Task<ActionResult<IEnumerable<InternViewModel>>> GetAll()
        {
            var result = await _mediator.Send(new BrowseInternsQuery());

            if (!result.Success)
                return StatusCode(result.StatusCode, EndpointResponse.Fail(result.StatusCode, "Faild To Get Interns", result.Errors));

            var internsVM = result.Data!.Select(x => new InternViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName = x.TrackName,
            });
            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<InternViewModel>>.OK(internsVM, result.StatusCode, "sucess"));
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterInternCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                var errorResponse = EndpointResponse<int>.Fail(
                    result.StatusCode,
                    "Intern registration failed",
                    result.Errors
                );

                return StatusCode(result.StatusCode, errorResponse);
            }

            var response = EndpointResponse<int>.OK(
                result.Data,
                result.StatusCode,
                "Intern registered successfully"
            );

            return StatusCode(result.StatusCode, response);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{

        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete()
        //{

        //}
    }
}
