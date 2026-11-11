
using LMS___Mini_Version.features.EnrollmentFeatures.CompleteEnrollment.Command;
using LMS___Mini_Version.features.EnrollmentFeatures.GetAllEnrollments.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollmentsHistory.GetEnrollmentsHistoryQuery;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;



        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult> GetById(int id)
        //{


        //}

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentsViewModel>>>> GetByIntern(int internId)
        {
            var result = await mediator.Send(new GetAllEnrollmentsQuery(internId));
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to retrieve enrollments",
                        result.Errors));
            }
            var enrollmentsVM = result.Data?.Select(x => new EnrollmentsViewModel
            {
                EnrollmentId = x.EnrollmentId,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.EnrollmentStatus,
                TrackId = x.TrackId,
                TrackName = x.TrackName
            });
            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<EnrollmentsViewModel>>.OK(enrollmentsVM, result.StatusCode, "Success"));
        }

        //[HttpPost]
        //public async Task<ActionResult<>> Enroll( )
        //{




        //}


        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> CompleteEnrollment(int id)
        {
            var result = await mediator.Send(new CompleteEnrollmentCommand(id));
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to complete enrollment",
                        result.Errors));
            }
            return StatusCode(result.StatusCode, EndpointResponse.OK(result.StatusCode, "Enrollment completed successfully"));
        }

        //[HttpPost("{id}/cancel")]
        //public async Task<ActionResult> Cancel()
        //{

        //}


        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackEnrollmentsViewModel>>>> GetTrackEnrollments(int trackId)
        {
            var result = await mediator.Send(new GetTrackEnrollmentsQuery(trackId));
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to retrieve track enrollments",
                        result.Errors));
            }
            var trackEnrollmentsVM = result.Data.Select(x => new TrackEnrollmentsViewModel
            {
                EnrollmentId = x.EnrollmentId,
                InternFullName = x.InternFullName,
                PaymentAmount = x.PaymentAmount,
                EnrollmentDate = x.EnrollmentDate,
                PaymentStatus = x.PaymentStatus,
                EnrollmentStatus = x.EnrollmentStatus
            });
            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<TrackEnrollmentsViewModel>>.OK(trackEnrollmentsVM, result.StatusCode, "Success"));
        }

        //[HttpPost("{id}/transfer/{newTrackId}")]
        //public async Task<ActionResult> Transfer()
        //{

        //}
    }
}
