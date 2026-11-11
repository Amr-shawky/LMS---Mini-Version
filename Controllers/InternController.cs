
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<int>>> Create([FromBody]AddInternCommand command , CancellationToken cancellationToken)
        {
            var result =await mediator.Send(command, cancellationToken);
            if (!result.Success)
                return StatusCode(result.StatusCode, EndpointResponse<int>.Fail(result.StatusCode, "Failed to register intern", result.Errors));
            return StatusCode(result.StatusCode, EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern registered successfully"));


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
