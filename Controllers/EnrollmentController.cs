
using LMS___Mini_Version.Features.EnrollmentFeature.Orchestrators;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {

        private readonly IMediator _mediator;
        
        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPut]
        public async Task<ActionResult<EndpointResponse<bool>>> TransferEnrollemnt(int enrollmentid , int newtrackid)
        {
            var result = await _mediator.Send(new TransferEnrollmentOrchestrator(enrollmentid, newtrackid));

            return Ok(EndpointResponse<bool>.OK(200, "success"));

        }

        
    }
}
