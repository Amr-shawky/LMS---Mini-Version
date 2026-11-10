
using LMS___Mini_Version.features.EnrollementFeature.Commands;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.EnrollementFeature.ViewModels;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator mediator;

        public EnrollmentController(IMediator mediator )
        {
            this.mediator = mediator;
        }
        // get by internId the enrollment
        [HttpGet]
        [Route("intern/{internId}")]

       public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollementsByInternIdViewModel>>>> GetAllEnrollemnentsByInternId( [FromRoute]int internId)
        {
          var response =  await  mediator.Send(new GetEnrollementWithInternIdQuery(internId));
           if(!response.Success || response.Data is null)
            {
                return StatusCode(response.StatusCode,EndpointResponse<IEnumerable<EnrollementsByInternIdViewModel>>.Fail(response.StatusCode,"Error Occured when trying to get the data",response.Errors));
            }
            else
            {
                var VM = response.Data.Select(x => new EnrollementsByInternIdViewModel()
                {
                    EnrollmentDate = x.EnrollmentDate,
                    EnrollmentId = x.EnrollmentId,
                    Status = x.Status,
                    TrackId = x.TrackId,
                    TrackName=x.TrackName,

                }).ToList();
                return StatusCode(response.StatusCode,EndpointResponse<IEnumerable<EnrollementsByInternIdViewModel>>.OK(VM, 200, "Data returned SucessFully"));
            }
        }

        // complete the enrollement
        [HttpPost]
        [Route("{Id}/complete")]

        public async Task<ActionResult<EndpointResponse>> CompleteEnrollemnent([FromRoute]int Id)
        {
            var response  = await mediator.Send(new CompleteEnrollementCommand(Id));   
            if(!response.Success )
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.StatusCode, "Cannot Complete the EnrollementionCompletion ", response.Errors));
            }
            else
            {
                return StatusCode(response.StatusCode, EndpointResponse.OK(response.StatusCode, "The Enrollement Completed Sucessfully"));
            }
        }



        [HttpGet]
        [Route("track/{trackId}")]

        public async Task<ActionResult<EndpointResponse<IEnumerable<EnrollementByTrackIdViewModel>>>> GetAllEnrollemnentsByTrackId([FromRoute] int trackId)
        {
            var response = await mediator.Send(new GetEnrollementByTrackIdQuery(trackId));
            if (!response.Success || response.Data is null)
            {
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<EnrollementByTrackIdViewModel>>.Fail(response.StatusCode, "Error Occured when trying to get the data", response.Errors));
            }
            else
            {
                var VM = response.Data.Select(x => new EnrollementByTrackIdViewModel()
                {
                    EnrollmentDate = x.EnrollmentDate,
                   
                    InternFullName = x.InternFullName,
                    PaymentAmount = x.PaymentAmount,
                    PaymentStatus = x.PaymentStatus,
                    enrollementStatus = x.enrollementStatus,
                    EnrollementId = x.EnrollementId,
                    TrackId = trackId
                        
                }).ToList();
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<EnrollementByTrackIdViewModel>>.OK(VM, 200, "Data returned SucessFully"));
            }
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
