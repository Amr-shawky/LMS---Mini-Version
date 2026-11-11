using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.Queries;
using LMS___Mini_Version.features.InternFeature.GetInternEnrollmentHistory.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.Queries;
using LMS___Mini_Version.features.TrackFeature.GetTrackEnrollments.ViewModels;
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetInternEnrollmentHistoryOutputViewModel>>>> GetInternEnrollmentHistory(int internId,CancellationToken cancellationToken)
        {
            if (internId <= 0)
            {
                var errorResponse=  EndpointResponse<IEnumerable<GetInternEnrollmentHistoryOutputViewModel>>.Fail(400, errors:new Dictionary<string, string[]> {{ "InternId", new[] { "Intern Id must be greater than zero" } } });
                return BadRequest(errorResponse);
            }

            var result = await _mediator.Send(new GetInternEnrollmentHistoryQuery(internId), cancellationToken);
            if (!result.Success)
            {
                var errorResponse = EndpointResponse<IEnumerable<GetInternEnrollmentHistoryOutputViewModel>>.Fail(result.StatusCode,errors: result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }
            var enrollmentHistoryVM = result.Data
                .Select(e => new GetInternEnrollmentHistoryOutputViewModel
                {
                    EnrollmentId = e.EnrollmentId,
                    TrackId = e.TrackId,
                    TrackName = e.TrackName,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.EnrollmentStatus
                }).ToList();

            var successResponse =EndpointResponse<IEnumerable<GetInternEnrollmentHistoryOutputViewModel>>.OK(enrollmentHistoryVM,200,"Enrollment history retrieved successfully.");
            return Ok(successResponse);
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> CompleteEnrollment(int id,CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                var errorResponse = EndpointResponse.Fail(400, errors: new Dictionary<string, string[]> { { "EnrollmentId", new[] { "Enrollment ID must be greater than zero" } } });
                return BadRequest(errorResponse);
            }

            var result = await _mediator.Send(new CompleteEnrollmentCommand(id), cancellationToken);
            if(!result.Success)
            {
                var errorResponse = EndpointResponse.Fail(result.StatusCode, errors: result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }

            var successResponse = EndpointResponse.OK(200, "Enrollment completed successfully.");
            return Ok(successResponse);
        }

        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetTrackEnrollmentsOutputViewModel>>>> GetTrackEnrollments(int trackId,CancellationToken cancellationToken)
        {
            if (trackId <= 0)
            {
                var errorResponse = EndpointResponse<IEnumerable<GetTrackEnrollmentsOutputViewModel>>.Fail(400, errors: new Dictionary<string, string[]> { { "TrackId", new[] { "Track ID must be greater than zero" } } });
                return BadRequest(errorResponse);
            }

            var result = await _mediator.Send(new GetTrackEnrollmentsQuery(trackId), cancellationToken);
            if (!result.Success)
            {
                var errorResponse = EndpointResponse<IEnumerable<GetTrackEnrollmentsOutputViewModel>>.Fail(result.StatusCode, errors: result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }
            var enrollmentsVM = result.Data
                .Select(e => new GetTrackEnrollmentsOutputViewModel
                {
                    InternFullName = e.InternFullName,
                    EnrollmentId = e.EnrollmentId,
                    EnrollmentDate = e.EnrollmentDate,
                    EnrollmentStatus = e.EnrollmentStatus,
                    PaymentAmount = e.PaymentAmount,
                    PaymentStatus = e.PaymentStatus
                }).ToList();

            var successResponse = EndpointResponse<IEnumerable<GetTrackEnrollmentsOutputViewModel>>.OK(enrollmentsVM,200);
            return Ok(successResponse);
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
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
