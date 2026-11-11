
using LMS___Mini_Version.Features.InternFeature.Dtos;
using LMS___Mini_Version.Features.InternFeature.Quries;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private IMediator _mediator;

        public InternController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllInternsWithEnrollment")]
        public async Task<ActionResult<EndpointResponse<Interndto>>> GetAllInternsWithEnrollment()
        {
            var result =await _mediator.Send(new GettAllInternsWithEnrollment(1,20));

            

            return Ok(result.Data);
        }
        
    }
}
