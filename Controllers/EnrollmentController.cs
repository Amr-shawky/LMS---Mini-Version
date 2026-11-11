
using LMS___Mini_Version.features.EnrollmentFeature.CompleteEnrollment.Commands;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentsByTrack.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetInternEnrollmentHistory.ViewModels;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator mediator;

        public EnrollmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> Complete(
                int id,
                CancellationToken cancellationToken)
        {
            if(id <= 0)
            {
                return BadRequest(
                    EndpointResponse.Fail(
                        400,
                        errors: new Dictionary<string, string[]>
                        {
                            ["EnrollmentId"] = new[]
                            {
                                "EnrollmentId must be greater than zero."
                            }
                        }
                    )
                );
            }
            var result = await mediator.Send(
        new CompleteEnrollmentCommand(id),
        cancellationToken);
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to complete enrollment",
                        result.Errors));
            }
            return Ok(
            EndpointResponse.OK(
            200,
            "Enrollment completed successfully"));

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
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentHistoryViewModel>>>> GetByIntern(
            int internId,
            CancellationToken ct)
        {
            if (internId <= 0)
            {
                return BadRequest(
                    EndpointResponse.Fail(
                        400,
                        errors: new Dictionary<string, string[]>
                        {
                            ["InternId"] = new[]
                            {
                                "InternId must be greater than zero."
                            }
                        }
                    )
                );
            }

            var result = await mediator.Send(
                new GetInternEnrollmentHistoryQuery(internId),
                ct);
            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to get enrollment history",
                        result.Errors
                    )
                );
            }
            var enrollmentViewModels = result.Data!
                .Select(x => new EnrollmentHistoryViewModel
                {
                    EnrollmentId = x.EnrollmentId,
                    TrackId = x.TrackId,
                    TrackName = x.TrackName,
                    EnrollmentDate = x.EnrollmentDate,
                    EnrollmentStatus = x.enrollmentStatus
                })
                .ToList();
            return StatusCode(
                result.StatusCode,
                EndpointResponse<IEnumerable<EnrollmentHistoryViewModel>>.OK(
                    enrollmentViewModels,
                    result.StatusCode,
                    "Enrollment history retrieved successfully"
                )
            );
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

        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentByTrackViewModel>>>> GetByTrack(
            int trackId,
            CancellationToken cancellationToken)
        {
            if (trackId <= 0)
            {
                return BadRequest(
                    EndpointResponse.Fail(
                        400,
                        errors: new Dictionary<string, string[]>
                        {
                            ["TrackId"] = new[]
                            {
                        "TrackId must be greater than zero."
                            }
                        }));
            }

            var result = await mediator.Send(
                new GetEnrollmentsByTrackQuery(trackId),
                cancellationToken);

            if (!result.Success)
            {
                return StatusCode(
                    result.StatusCode,
                    EndpointResponse.Fail(
                        result.StatusCode,
                        "Failed to get enrollments by track",
                        result.Errors));
            }

            var enrollmentViewModels = result.Data!
                .Select(x => new EnrollmentByTrackViewModel
                {
                    EnrollmentId = x.EnrollmentId,
                    InternName = x.InternName,
                    EnrollmentDate = x.EnrollmentDate,
                    EnrollmentStatus = x.EnrollmentStatus,
                    PaymentAmount = x.PaymentAmount,
                    PaymentStatus = x.PaymentStatus
                })
                .ToList();

            return Ok(
                EndpointResponse<IEnumerable<EnrollmentByTrackViewModel>>.OK(
                    enrollmentViewModels,
                    200,
                    "Enrollments retrieved successfully"));
        }

    }
}
