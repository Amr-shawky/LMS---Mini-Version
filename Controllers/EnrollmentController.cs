
using LMS___Mini_Version.features.EnrollmentFeature.CompleteTraining;
using LMS___Mini_Version.features.EnrollmentFeature.GetTrackEnrollments;
using LMS___Mini_Version.features.EnrollmentFeature.InternEnrollment;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/Enrollment")]
    public class EnrollmentController : ControllerBase
    {

       readonly private IMediator _mediator;
        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternEnrollmentViewModel>>>> GetByIntern(int internId)
        {
            RequestResponse<IEnumerable<InternEnrollmentDto>> response = await _mediator.Send(new GatInternEnrollmentQuery(internId));
            if(!response.Success)
            {
                return EndpointResponse<IEnumerable<InternEnrollmentViewModel>>.Fail(response.StatusCode, errorCode: response.ErrorCode);
            }
            IEnumerable < InternEnrollmentViewModel > enrollmentViewModels = response.Data.Select(e => new InternEnrollmentViewModel
            {
                EnrollmentId = e.EnrollmentId,
                TrackName = e.TrackName,
                EnrollmentStatus = e.EnrollmentStatus,
                EnrollmentDate = e.EnrollmentDate
            });
          
            return EndpointResponse<IEnumerable<InternEnrollmentViewModel>>.OK(enrollmentViewModels, 200);
        }
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> CompleteTraining(int id)
        {
            RequestResponse response = await _mediator.Send(new CompleteTrainingCommand(id));
            if (!response.Success)
            {
                return EndpointResponse.Fail(response.StatusCode, errorCode: response.ErrorCode);
            }
            return EndpointResponse.OK(response.StatusCode);
        }

        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackEnrollmentsViewModel>>>> GetAll(int trackId)
        {
            RequestResponse<IEnumerable<TrackEnrollmentsDto>> response = await _mediator.Send(new TrackEnrollmentsQuery(trackId));
            if(!response.Success)
            {
                return EndpointResponse<IEnumerable<TrackEnrollmentsViewModel>>.Fail(response.StatusCode, errorCode: response.ErrorCode);
            }
            return EndpointResponse<IEnumerable<TrackEnrollmentsViewModel>>.OK(response.Data.Select(e => new TrackEnrollmentsViewModel
            {
                EnrollmentId = e.EnrollmentId,
                InternFullName = e.InternFullName,
                EnrollmentStatus = e.EnrollmentStatus,
                EnrollmentDate = e.EnrollmentDate,
                PaymentAmount = e.PaymentAmount,
                PaymentStatus = e.PaymentStatus
            }), 200);

        }

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
