using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.Queries;
using LMS___Mini_Version.features.TrackFeature.AvailabilityTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackViewModel>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTracksQuery(1, 20));

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get tracks", result.Errors));

            var tracksVm = result.Data!.Select(x => new TrackViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            
            return StatusCode(result.StatusCode,EndpointResponse<IEnumerable<TrackViewModel>>.OK(tracksVm, result.StatusCode,"sucess"));

        }

        [HttpGet("{id:int}/availability")]
        public async Task<ActionResult<EndpointResponse<TrackAvailabilityViewModel>>> GetAvailability(
            int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AvailabilityQuery(id), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<TrackAvailabilityViewModel>.Fail(result.StatusCode, "Failed to get availability", result.Errors));

            var d = result.Data!;
            var vm = new TrackAvailabilityViewModel(
                d.TrackName, d.IsActive, d.MaxCapacity, d.ReservedSeats, d.AvailableSeats);

            return StatusCode(result.StatusCode,
                EndpointResponse<TrackAvailabilityViewModel>.OK(vm, result.StatusCode));
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById()
        //{

        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create(
          [FromBody] AddTrackCommand command)
        {
            var result = await _mediator.Send(command);

            var response = EndpointResponse.OK( 201,
                result.Success ? "Track created successfully" : "Failed to create track"
            );

            return StatusCode(response.StatusCode, response);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{



        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete()
        //{

        //}

    }
}
