
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Commands;
using LMS___Mini_Version.Features.TrackFeature.Dtos;
using LMS___Mini_Version.Features.TrackFeature.Queries;
using LMS___Mini_Version.Features.TrackFeature.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TrackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackViewModel>>>> GetAll(int pageindex, int pagesize)
        {
            var tracks = await _mediator.Send(new GetAllTracksQuery(pageindex, pagesize));

            var trackVMS = tracks.Data.Select(x => new TrackViewModel
            {
                id = x.id,
                Fees = x.Fees,
                Name = x.Name
            });
            int statuscode = tracks.StatusCode;



            return StatusCode(statuscode,
                EndpointResponse<IEnumerable<TrackViewModel>>.
                OK(statuscode,trackVMS,"track retreived successfuly"));

        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create(CreateTrackViewModel vm)
        {
            //

            var result = await _mediator.Send(new CreateTrackCommand(vm.Name , vm.Fees,vm.MaxCapacity));

            return StatusCode(result.StatusCode,EndpointResponse.OK(result.StatusCode,"creation successfully"));
            
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<EndpointResponse>> update(int id , decimal fees)
        {
            var result = await _mediator.Send(new UpdateTrackFeesCommand(fees,id));
            return StatusCode(result.StatusCode, EndpointResponse.OK(result.StatusCode,"updated successfully"));
        }
    }
}
