
using LMS___Mini_Version.features.InternFeature.BrowseInterns;
using LMS___Mini_Version.features.InternFeature.RegisterIntern;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/intern")]
    public class InternController(IMediator mediator) : ControllerBase
    {

     
        
        [HttpPost]
        public async Task<ActionResult<EndpointResponse<RegisterViewModel>>> Create([FromBody] RegisterInternCommand registerInternCommand)
        {

            RequestResponse<RegisterDto> response = await mediator.Send(registerInternCommand);
            if(response.Success)
                return StatusCode(response.StatusCode, EndpointResponse<RegisterViewModel>.OK( new RegisterViewModel {internId =response.Data.internId }, response.StatusCode));


            return StatusCode(response.StatusCode, EndpointResponse<RegisterViewModel>.Fail(response.StatusCode, errorCode: response.ErrorCode));

        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<BrowseInternsViewModel>>>> GetAll()
        {
            RequestResponse<IEnumerable<BrowseInternsDto>>? response = await mediator.Send(new BrowseInternsQuery());
            return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<BrowseInternsViewModel>>.OK(response.Data?.Select(i => new BrowseInternsViewModel
            {
                InternId = i.InternId,
                FullName = i.FullName,
                Email = i.Email,
                Status = i.Status,
                TrackName = i.TrackName
            }), response.StatusCode));

        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
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
