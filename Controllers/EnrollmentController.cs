
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.Queries;
using LMS___Mini_Version.features.EnrollmentFeatures.GetInternById.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.Queries;
using LMS___Mini_Version.features.EnrollmentFeatures.GetTrackEnrollments.ViewModels;
using LMS___Mini_Version.features.EnrollmentFeatures.UpdateStatusEnrollment.Commands;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(IMediator mediator) : ControllerBase
    {



        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackEnrollmentViewModel>>>> GetById(int trackId)
        {
            var result = await mediator.Send(new GetTrackEnrollmentsQuery(trackId));

            var vm = result.Data!.Select(x=> new TrackEnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.EnrollmentStatus,
                InternFullName = x.InternFullName,
                PaymentAmount = x.PaymentAmount,
                PaymentStatus = x.PaymentStatus,
            });


           var response =  EndpointResponse < IEnumerable<TrackEnrollmentViewModel>>.OK(vm,200,
                result.Success ? "Reteriving data success" : " Error about retriving");

            return StatusCode(result.StatusCode, response);
        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollmentViewModel>>>>
            GetByIntern(int internId)
        {
            var result = await mediator.Send(new GetByInternQuery(internId));
            var vm = result.Data!.Select(x=> new EnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                EnrollmentDate = x.EnrollmentDate,
                EnrollmentStatus = x.EnrollmentStatus,
                TrackId = x.TrackId,
                TrackName = x.TrackName,
            });

            var response = EndpointResponse<IEnumerable<EnrollmentViewModel>>.OK(vm, result.StatusCode,
                result.Success ? "Reteriving all interns with Enrollments" :  "Error");

            return StatusCode(result.StatusCode, response);
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<EndpointResponse>> Enroll(int id)
        {

            var result = await mediator.Send(new UpdateStatusEnrollmentCommand(id));

            var response = EndpointResponse<EnrollmentViewModel>.OK(result.StatusCode,
                result.Success ? "Updating Complate " : "Error");

            return StatusCode(result.StatusCode, response);


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
