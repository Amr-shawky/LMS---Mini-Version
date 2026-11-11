using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.AddTrack.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.Queries;
using LMS___Mini_Version.features.TrackFeature.GetTrackAvailability.ViewModels;
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackViewModel>>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllTracksQuery(1, 20), cancellationToken);

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

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create([FromBody] AddTrackInputViewModel vm,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddTrackCommand(vm.Name,vm.Fees,vm.MaxCapacity), cancellationToken);
            if(!result.Success)
            {
                var errorResponse = EndpointResponse.Fail(result.StatusCode, "Failed to create track", result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }
            var response = EndpointResponse.OK(201,
                result.Success ? "Track created successfully" : "Failed to create track");

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<EndpointResponse<GetTrackAvailabilityOutputViewModel>>> GetAvailability(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                var errorResponse = EndpointResponse<GetTrackAvailabilityOutputViewModel>.Fail(400, "TrackId must be greater than 0");
                return BadRequest(errorResponse);
            }

            var result = await _mediator.Send(new GetTrackAvailabilityQuery(id), cancellationToken);
            if(!result.Success)
            {
                var errorResponse = EndpointResponse<GetTrackAvailabilityOutputViewModel>.Fail(result.StatusCode, errors:result.Errors);
                return StatusCode(result.StatusCode, errorResponse);
            }
            var TrackAvailabilityVM = new GetTrackAvailabilityOutputViewModel
            {
                TrackName = result.Data.TrackName,
                IsActive = result.Data.IsActive,
                MaxCapacity = result.Data.MaxCapacity,
                ReservedSeats = result.Data.ReservedSeats,
                AvailableSeats = result.Data.AvailableSeats
            };

            var successResponse = EndpointResponse<GetTrackAvailabilityOutputViewModel>.OK(TrackAvailabilityVM, 200, "Track availability retrieved successfully.");
            return StatusCode(successResponse.StatusCode, successResponse);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById()
        //{

        //}

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
