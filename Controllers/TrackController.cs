
using LMS___Mini_Version.Attributes;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackByAvailability.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<TrackViewModel>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTracksQuery(1, 20));

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get tracks", result.Errors));

            var tracksVM = result.Data!.Select(x => new TrackViewModel
            {
                id = x.Id,
                name = x.Name
            });

            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<TrackViewModel>>.OK(tracksVM, result.StatusCode, "sucess"));

        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{

        //}

        [HttpGet("{TrackId}/availability")]
        public async Task<ActionResult<EndpointResponse<GetTrackByAvailabilityViewModel>>> GetById(int TrackId)
        {
            var result = await _mediator.Send(new GetTrackByAvailabilityQuery(TrackId));
            if (!result.Success)
                return EndpointResponse<GetTrackByAvailabilityViewModel>.Fail(result.StatusCode, "An Error Occured", result.Errors);
            var dtoResult = result.Data;
            var resultVm = new GetTrackByAvailabilityViewModel(dtoResult!.TrackName, dtoResult.IsActive, dtoResult.ReservedSeats, dtoResult.MaxCapacity, dtoResult.AvailableSeats);
            return EndpointResponse<GetTrackByAvailabilityViewModel>.OK(resultVm, result.StatusCode, "sucess");
        }
        [SaveChanges]
        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create(AddTrackCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return EndpointResponse<GetTrackByAvailabilityViewModel>.Fail(result.StatusCode, "An Error Occured", result.Errors);
            return EndpointResponse<GetTrackByAvailabilityViewModel>.OK(result.StatusCode, "sucess");

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
