
using LMS___Mini_Version.features.InternFeature.AddIntern.Command;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

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
