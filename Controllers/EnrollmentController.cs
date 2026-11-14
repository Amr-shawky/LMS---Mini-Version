
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.shared;
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

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentDto>>>> GetByIntern(int internId)
        {
            var query = new GetAllEnrollmentQuery(internId);

            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                var errorResponse = EndpointResponse<IEnumerable<EnrollmentDto>>.Fail(
                    result.StatusCode,
                    "Intern not found.",
                    result.Errors
                );

                return StatusCode(result.StatusCode, errorResponse);
            }

            var response = EndpointResponse<IEnumerable<EnrollmentDto>>.OK(
                result.Data!,
                200,
                "Enrollment history retrieved successfully."
            );

            return Ok(response);
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        //[HttpGet("intern/{internId}")]
        //public async Task<ActionResult<IEnumerable<>>> GetByIntern()
        //{

        //}

        //[HttpPost]
        //public async Task<ActionResult<>> Enroll( )
        //{




        //}

        //[HttpPost("{id}/cancel")]
        //public async Task<ActionResult> Cancel()
        //{

        //}

        //[HttpPost("{id}/transfer/{newTrackId}")]
        //public async Task<ActionResult> Transfer()
        //{

        //}
    }
}
