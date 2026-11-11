
using LMS___Mini_Version.features.EnrollmentsFeature.Queries;
using LMS___Mini_Version.features.EnrollmentsFeature.ViewModels;
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

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<InternEnrollmentHistoryViewModel>>> GetInternEnrollmentHistory(int internId, CancellationToken cancellationToken)
        {
            var query = new GetInternEnrollmentHistoryQuery(internId);
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse<InternEnrollmentHistoryViewModel>.Fail(result.StatusCode, "Failed to retrieve enrollment history.", result.Errors));
            }
            var enrollmentHistoryViewModels = result.Data.Select(e => new InternEnrollmentHistoryViewModel
            {
                EnrollmentId = e.EnrollmentId,
                TrackId = e.TrackId,
                TrackName = e.TrackName,
                EnrollmentDate = e.EnrollmentDate,
                EnrollmentStatus = e.EnrollmentStatus.ToString()
            });
           return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<InternEnrollmentHistoryViewModel>>.OK(enrollmentHistoryViewModels, result.StatusCode, "Enrollment history retrieved successfully."));
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
