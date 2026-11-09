using LMS___Mini_Version.features.EnrollmentFeature.Command.UpdateStatus;
using LMS___Mini_Version.features.EnrollmentFeature.Queries.GetInternEnrollments;
using LMS___Mini_Version.features.EnrollmentFeature.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.TrackEnrollments.Query;
using LMS___Mini_Version.features.TrackFeature.TrackEnrollments.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(IMediator mediator) : ControllerBase
    {

        [HttpPost("{id:int}/complete")]
        public async Task<ActionResult<EndpointResponse>> Complete(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CompleteEnrollmentCommand(id), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to complete enrollment", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse.OK(result.StatusCode, "Enrollment completed successfully"));
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{
            
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{
           
        //}

        // Task 03
        [HttpGet("intern/{internId:int}")]
        public async Task<ActionResult<EndpointResponse<List<EnrollmentViewModel>>>> GetByIntern(
            int internId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetInternEnrollmentsQuery(internId), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<List<EnrollmentViewModel>>.Fail(result.StatusCode, "Failed to get enrollments", result.Errors));

            var enrollments = (result.Data ?? []).Select(e => new EnrollmentViewModel(
                e.EnrollmentId, e.TrackId, e.TrackName, e.EnrollmentDate, e.EnrollmentStatus)).ToList();

            return StatusCode(result.StatusCode,
                EndpointResponse<List<EnrollmentViewModel>>.OK(enrollments, result.StatusCode));
        }
        
        // Task 07
        [HttpGet("track/{trackId:int}")]
        public async Task<ActionResult<EndpointResponse<List<TrackEnrollmentViewModel>>>> GetByTrack(
            int trackId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetTrackEnrollmentsQuery(trackId), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<List<TrackEnrollmentViewModel>>.Fail(
                        result.StatusCode, "Failed to get track enrollments", result.Errors));

            var enrollments = (result.Data ?? []).Select(e => new TrackEnrollmentViewModel(
                e.EnrollmentId,
                e.InternFullName,
                e.EnrollmentDate,
                e.EnrollmentStatus,
                e.PaymentAmount,
                e.PaymentStatus)).ToList();

            return StatusCode(result.StatusCode,
                EndpointResponse<List<TrackEnrollmentViewModel>>.OK(enrollments, result.StatusCode));
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