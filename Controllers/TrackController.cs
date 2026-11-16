
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.TrackAvailability.Queries;
using LMS___Mini_Version.features.TrackFeature.TrackAvailability.ViewModels;
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

            var tracksVM = result.Data.Select(x => new TrackViewModel
            {
                id = x.id,
                name = x.Name
            });
            
            return StatusCode(result.StatusCode,EndpointResponse<IEnumerable<TrackViewModel>>.OK(tracksVM, result.StatusCode,"sucess"));

        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        [HttpGet("{TrackId}/availability")]
        public async Task<ActionResult<EndpointResponse<TrackAvailabilityViewModel>>> Get(int TrackId)
        {
            var result = await _mediator.Send(new TrackAvailabilityQuery(TrackId));

            var vm = new TrackAvailabilityViewModel
            {
                AvailableSeats = result.Data!.AvailableSeats,
                ReservedSeats = result.Data.ReservedSeats,
                IsActive = result.Data.IsActive,
                MaxCapacity = result.Data.MaxCapacity,
                TrackName = result.Data.TrackName,
            };
            return StatusCode(result.StatusCode, EndpointResponse<TrackAvailabilityViewModel>.OK(vm, result.StatusCode, "sucess"));


        }

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
