using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Enrollments.VMs;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.Shared.LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Features.Tracks.VMs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator Mediator;

        public EnrollmentController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("InternId")]
        public async Task<ActionResult<IEnumerable<ViewInternEnrollmentHistoryDTO>>> GetInternEnrollmentHistory(int id,CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetInternEnrollmentHistoryQuery(id), cancellationToken);
            if (result is null)
                return NotFound("Intern is not found");
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<RequestResponse>> CompleteEnrollment(CompleteTrainingEnrollmentCommand command,CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(command, cancellationToken);
            if (result is null)
                return NotFound("Enrollment is Unkown or Completed");
            return result;
        }
        [HttpGet("track/{trackId}")]
        public async Task<EndpointResponse<IEnumerable<ReviewTrackEnrollmentViewModel>>> GetTrackEnrollments(int trackId, CancellationToken cancellationToken)
        {
            var queries= await Mediator.Send(new ReviewTrackEnrollments(trackId), cancellationToken);
            if(!queries.Success)
                return EndpointResponse<IEnumerable<ReviewTrackEnrollmentViewModel>>.Fail(404, "Track is not found");

            var ViewModels = new List<ReviewTrackEnrollmentViewModel>();
            foreach (var item in queries.Data)
            {
                ViewModels.Add(
                    new ReviewTrackEnrollmentViewModel
                    {
                        EnrollmentId = item.EnrollmentId,
                        InternFullName = item.InternFullName,
                        EnrollmentDate = item.EnrollmentDate,
                        enrollmentStatus = item.enrollmentStatus,
                        PaymentAmount = item.PaymentAmount,
                        paymentStatus = item.paymentStatus
                    });
            }

            var result = EndpointResponse<IEnumerable<ReviewTrackEnrollmentViewModel>>.OK(ViewModels);
            return result;
        }
    }
}
