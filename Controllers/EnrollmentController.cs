
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.EnrollmentFeature.Queries;
using LMS___Mini_Version.features.EnrollmentFeature.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternEnrollmentViewModel>>>> GetByIntern(int internId)
        {
            var result = await _mediator.Send(new GetInternEnrollmentHistoryQuery(internId));

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get enrollments", result.Errors));

            var enrollmentVM = result.Data.Select(x => new InternEnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                TrackId = x.TrackId,
                TrackName = x.TrackName,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.EnrollmentStatus
            });

            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<InternEnrollmentViewModel>>.OK(enrollmentVM, result.StatusCode, "sucess"));

        }

        //[HttpPost]
        //public async Task<ActionResult<>> Enroll( )
        //{




        //}


        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> Complete(int id)
        {
            var result = await _mediator.Send(new CompleteEnrollmentCommand(id));

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to complete enrollment", result.Errors));


            return StatusCode(result.StatusCode, EndpointResponse.OK(result.StatusCode, "sucess"));

        }


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
