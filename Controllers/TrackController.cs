
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.CheckTrack.Queries;
using LMS___Mini_Version.features.TrackFeature.CheckTrack.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Dtos;
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

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<EndpointResponse<CheckTrackViewModel>>> CheckTrackAvailablity(int id)
        {
            var result = await _mediator.Send(new CheckTrackQuery(id));
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get track availability", result.Errors));
            var track = result.Data;
            var trackVM = new CheckTrackViewModel
            {
                TrackName = track.Name,
                IsActive = track.IsActive,
                MaxCapacity = track.MaxCapacity,
                ReservedSeats = track.ReservedSeats,
                AvailableSeats = track.AvailableSeats
            };
            return StatusCode(result.StatusCode, EndpointResponse<CheckTrackViewModel>.OK(trackVM, result.StatusCode, "success"));
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete()
        //{

        //}

    }
}
