using LMS___Mini_Version.Attributes;
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.Query;
using LMS___Mini_Version.features.EnrollmentFeature.GetAllEnrollment.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.Quey;
using LMS___Mini_Version.features.EnrollmentFeature.GetEnrollmentTrackIntern.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeature.UpdateEnrollmentStatus.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(IMediator mediator) : ControllerBase
    {




        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetEnrollmentTrackInternViewModel>>>> GetAll(int trackId)
        {
            var result = await mediator.Send(new GetEnrollmentTrackInternQuery(trackId));
            if (!result.Success)
                return EndpointResponse<IEnumerable<GetEnrollmentTrackInternViewModel>>.Fail(result.StatusCode, "Error Or Multiple Errors Occured", result.Errors!);
            var EnrollViewModel = result.Data!.Select(s => new GetEnrollmentTrackInternViewModel(s.EnrollmentId, s.InternFullName, s.EnrollmentDate, s.EnrollmentStatus, s.PaymentAmount, s.PaymentStatus!));
            return EndpointResponse<IEnumerable<GetEnrollmentTrackInternViewModel>>.OK(EnrollViewModel);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetAllEnrollmentsViewModel>>>> GetByIntern(int internId)
        {
            var result = await mediator.Send(new GetAllEnrollmentQuery(internId));
            if (!result.Success)
                return EndpointResponse<IEnumerable<GetAllEnrollmentsViewModel>>.Fail(result.StatusCode, "Error Or Multiple Errors Occured", result.Errors!);
            var EnrollViewModel = result.Data!.Select(s => new GetAllEnrollmentsViewModel(s.EnrollmentId, s.TrackId, s.EnrollmentDate, s.EnrollmentStatus, s.TrackName));
            return EndpointResponse<IEnumerable<GetAllEnrollmentsViewModel>>.OK(EnrollViewModel);
        }
        [SaveChanges]
        [HttpPost("{enrollmentId}/EnrollComplete")]
        //[HttpPost("{enrollmentId}/EnrollComplete")]
        public async Task<EndpointResponse> EnrollComplete(int enrollmentId)
        {
            var result = await mediator.Send(new UpdateEnrollmentStatusCommand(enrollmentId));
            if (!result.Success)
                return EndpointResponse.Fail(result.StatusCode, "Error Occured", result.Errors);

            return EndpointResponse.OK(result.StatusCode, $"Enrollment with Id {enrollmentId} has been updated to complete");
        }




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
