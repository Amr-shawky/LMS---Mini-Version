
using Azure;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.InternFeature.ViewModels;
using LMS___Mini_Version.features.shared;
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternViewModel>>>> GetAll()
        {
            var query = new BrowseInternsQuery();
            var result = await _mediator.Send(query);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<InternViewModel>>.Fail(result.StatusCode, "Cannot Get Interns", result.Errors));
            }
            var internsViewModel = result.Data!.Select(i => new InternViewModel()
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                Status = i.Status,
                TrackName = i.TrackName
            });
            return StatusCode(result.StatusCode, EndpointResponse<IEnumerable<InternViewModel>>.OK(internsViewModel , result.StatusCode, "Interns retrieved successfully"));
        }




        [HttpPost]
        public async Task<ActionResult<EndpointResponse<RegisterInterViewModel>>> Create(RegisterInterViewModel intern)
        {
            var command = new RegisterInternCommand(
                intern.FullName,
                intern.Email,
                intern.BirthYear,
                intern.TrackId
            );
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, EndpointResponse<RegisterInterViewModel>.Fail(result.StatusCode, "Cannot Register Intern To Track", result.Errors));
            }
            var CreateInternViewModel = new CreateInternViewModel()
            {
                internId = result.Data!.internId,
            };
            return StatusCode(result.StatusCode, EndpointResponse<CreateInternViewModel>.OK(CreateInternViewModel, result.StatusCode, "Interned created with track Sucessfully"));
        }




        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<>>> GetAll()
        //{
            
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{
           
        //}

        //[HttpPost]
        //public async Task<ActionResult<>> Create( )
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
