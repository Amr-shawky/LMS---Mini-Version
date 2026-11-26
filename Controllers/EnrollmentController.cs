
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollments.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Dtos;
using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments.Query;
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

        // Task 03 — Intern enrollment history (all statuses, newest first)
        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternEnrollmentHistoryDto>>>> GetByIntern(
            int internId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetInternEnrollmentsQuery(internId), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get enrollment history", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<IEnumerable<InternEnrollmentHistoryDto>>.OK(result.Data!, result.StatusCode, "success"));
        }

        // Task 07 — Enrollments of a track with intern name and payment info
        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackEnrollmentDto>>>> GetByTrack(
            int trackId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTrackEnrollmentsQuery(trackId), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get track enrollments", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<IEnumerable<TrackEnrollmentDto>>.OK(result.Data!, result.StatusCode, "success"));
        }

        //[HttpPost]
        //public async Task<ActionResult<>> Enroll( )
        //{

        //}

        // Task 04 — Mark an Active, fully paid enrollment as Completed
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> Complete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompleteEnrollmentCommand(id), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Enrollment could not be completed", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse.OK(result.StatusCode, $"Enrollment {id} marked as Completed"));
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
