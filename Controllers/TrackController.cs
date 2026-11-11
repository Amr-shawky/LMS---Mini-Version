
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Command;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Dtos;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.Query;
using LMS___Mini_Version.features.TrackFeature.TrackAvaillability.ViewModules;
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
                name = x.name
            });
            
            return StatusCode(result.StatusCode,EndpointResponse<IEnumerable<TrackViewModel>>.OK(tracksVM, result.StatusCode,"sucess"));

        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById()
        //{

        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create([FromBody] AddTrackCommand command)
        {
            var result = await _mediator.Send(command);

            var response = EndpointResponse.OK( 201,
                result.Success ? "Track created successfully" : "Failed to create track"
            );

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{trackId}/availability")]
        public async Task<IActionResult> GetTrackAvailability(int trackId)
        {
            var result = await _mediator.Send(new GetTrackAvailabilityQuery(trackId));
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse.Fail(result.StatusCode, " Failed to get track availability."));
            }

            var responceData = new TrackAvailabilityViewModule()
            {
                TrackName = result.Data!.TrackName,
                IsActive = result.Data!.IsActive,
                MaxCapacity = result.Data!.MaxCapacity,
                ReservedSeats = result.Data!.ReservedSeats,
                AvailableSeats = result.Data!.AvailableSeats,
            };

            return StatusCode(result.StatusCode, EndpointResponse<TrackAvailabilityViewModule>.OK(responceData, result.StatusCode, "Track availability retrieved successfully."));
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
