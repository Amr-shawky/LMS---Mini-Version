
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.InternFeature.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.Query;
using LMS___Mini_Version.features.TrackFeature.GetAllTracks.ViewModels;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InternController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllInternsViewModel>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get interns", result.Errors));

            if(!result.Data.Any())
                return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<AllInternsViewModel>>.OK([], result.StatusCode, "no interns found"));


            var internsVM = result.Data.Select(x => new AllInternsViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                TrackName = x.TrackName
            });

            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<AllInternsViewModel>>.OK(internsVM, result.StatusCode, "sucess"));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EndpointResponse<GetInternByIdViewModel>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetInternByIdQuery(id));

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to get intern", result.Errors));

            var intern = result.Data;
            var internVM = new GetInternByIdViewModel()
            {
                FullName = intern.FullName,
                Email = intern.Email,
                Status = intern.Status,
                BirthYear = intern.BirthYear,
            };

            return StatusCode(result.StatusCode, EndpointResponse<GetInternByIdViewModel>.OK(internVM, result.StatusCode, "sucess"));

        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse>> Create(CreateInternViewModel viewModel)
        {
            var result = await _mediator.Send(new CreateInternCommand(viewModel.FullName,
                viewModel.Email, viewModel.BirthYear, viewModel.TrackId));
            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to register intern", result.Errors));

            var response = EndpointResponse.OK(201,
                 $"Intern with Id {result.Data.Id} registered successfully");

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
