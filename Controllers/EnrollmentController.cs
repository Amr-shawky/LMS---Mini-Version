
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.ViewModules;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpGet("intern/{internId:int}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentHistoryByInternViewModule>>>> GetByIntern(int internId)
        {
            var result = await _mediator.Send(new GetInternEnrollmentHistoryQuery(internId));
            if(!result.Success)
            {
                return StatusCode
                (
                        result.StatusCode,
                        EndpointResponse<IEnumerable<EnrollmentHistoryResponseDto>>.Fail
                        (
                            result.StatusCode, 
                            "Faild To Load Enrollment History OF This Intern"
                        )
                );
            }


            var response = EndpointResponse<IEnumerable<EnrollmentHistoryResponseDto>>.OK
                        (
                            result.Data!,
                            result.StatusCode,
                            "Enrollment history retrieved successfully."
                        );


            return StatusCode(result.StatusCode, response);
        }

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
