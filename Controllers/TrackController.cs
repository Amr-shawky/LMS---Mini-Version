
using Azure;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTOs;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Queries;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.ViewModels;
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


        [HttpGet("{id:int}/availability")]
        public async Task<ActionResult<EndpointResponse<CheckTrackAvaiabilityViewModel>>> CheckAvailability(int id)
        {
            var result = await _mediator.Send(new CheckTrackAvaiabilityQuery(id));

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse<CheckTrackAvaiabilityViewModel>.Fail(result.StatusCode, "Failed to check Track Avaiabiliity", result.Errors));
            }
            else
            {
                var trackAvailabilityVM = new CheckTrackAvaiabilityViewModel
                {
                    TrackName = result.Data.TrackName,
                    MaxCapacity = result.Data.MaxCapacity,
                    ReservedSeats = result.Data.ReservedSeats,
                    AvailableSeats = result.Data.AvailableSeats,
                    IsActive = result.Data.IsActive
                };
                return StatusCode(result.StatusCode, EndpointResponse<CheckTrackAvaiabilityViewModel>.OK(trackAvailabilityVM, result.StatusCode, "Success"));

            }
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
