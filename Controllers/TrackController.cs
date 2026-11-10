
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetActiveTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllActiveTracks.ViewModels;
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
        //[HttpGet]
        //public async Task<ActionResult<EndpointResponse<IEnumerable<TrackViewModel>>>> GetAll()
        //{
        //    var result = await _mediator.Send(new CheckTrackStatusQuery(1, 20));

        //    if (!result.Success)
        //        return StatusCode(result.StatusCode,
        //            EndpointResponse.Fail(result.StatusCode, "Failed to get tracks", result.Errors));

        //    var tracksVM = result.Data.Select(x => new TrackViewModel
        //    {
        //        id = x.id,
        //        name = x.name
        //    });
            
        //    return StatusCode(result.StatusCode,EndpointResponse<IEnumerable<TrackViewModel>>.OK(tracksVM, result.StatusCode,"sucess"));

        //}

        [HttpGet]
        [Route("{id}/availability")]
        public async Task<ActionResult<EndpointResponse<CheckTrackAvaiabilityViewModel>>> CheckTrackAvaiability([FromRoute] int id)
        {
            var response = await _mediator.Send(new CheckTrackAvaiabilityQuery(id));
            if(!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse<CheckTrackAvaiabilityViewModel>.Fail(response.StatusCode, "Failed to check Track Avaiabiliity",response.Errors));
            }
            else
            {
                var vm = new CheckTrackAvaiabilityViewModel()
                {
                    AvailableSeats = response.Data.AvailableSeats,
                    IsActive = response.Data.IsActive,
                    MaxCapacity = response.Data.MaxCapacity,
                    ReservedSeats = response.Data.ReservedSeats,
                    TrackName = response.Data.TrackName,    
                };
                return StatusCode(response.StatusCode, EndpointResponse<CheckTrackAvaiabilityViewModel>.OK(vm, response.StatusCode, "SucessFully Check Track Avaiability"));
            }
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

        [HttpGet]
        [Route("Active")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<ActiveTrackViewModel>>>> GetAllActiveTracks()
        {
            var response = await _mediator.Send(new GetActiveTracksQuery());
            if(!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.StatusCode, "Cannot return Active Tracks", response.Errors));
            }
            else
            {
                var vm = response.Data!.Select(x => new ActiveTrackViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,  
                });
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<ActiveTrackViewModel>>.OK(vm , response.StatusCode, "Get Active Track Sucessfully"));

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
