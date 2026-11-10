
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.InternFeature.ViewModels;
using LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.Commands;
using LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.ViewModels;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.DTO;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator mediator;

        public InternController(IMediator mediator )
        {
            this.mediator = mediator;
        }

        [HttpGet]
        
        public async Task<ActionResult<EndpointResponse<IEnumerable<InternsWithTheirTracksViewModel>>>> GetAll()
        {
            var response =  await mediator.Send(new GetInternsWithTrackQuery());
            if (!response.Success || response.Data is null)
                return StatusCode(response.StatusCode, EndpointResponse.Fail(response.StatusCode, "Error Occured when retrieving the data", response.Errors));
            else

            {
               var VM= response.Data.Select(x => new InternsWithTheirTracksViewModel()
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    Id = x.Id,
                    Status   = x.Status,    
                    TrackName = x.TrackName,

                }).ToList();
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<InternsWithTheirTracksViewModel>>.OK(VM));
                   
            }
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}

        [HttpPost]
        
        public async Task<ActionResult<EndpointResponse<CreateInternViewModel>>> Create ([FromBody]AssignInternWithTrackViewModel intern)
        {
            var response = await  mediator.Send(new AssignInternIntoTrackCommand(intern.FullName  , intern.Email , intern.BirthYear, intern.TrackId));
            if(!response.Success)
            {
                return StatusCode(response.StatusCode , EndpointResponse.Fail(response.StatusCode ,"Cannot Assign Intern to track" ,response.Errors));
            }
            var CreateInternViewModel = new CreateInternViewModel()
            {
                Id = response.Data!.Id
            };
            return StatusCode(response.StatusCode, EndpointResponse<CreateInternViewModel>.OK( CreateInternViewModel,response.StatusCode, "Interned created with track Sucessfully"));
            


        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<ActionResult<EndpointResponse>> DeleteIntern([FromRoute]int id)
        {
            var response = await mediator.Send(new DeleteInternCommand(id));
            if(!response.Success)
            {
                return EndpointResponse.Fail(response.StatusCode, $"Failed to Delete intern with id:{id}", response.Errors);
            }
            else
            {
                return EndpointResponse.OK(response.StatusCode, $"Intern With Id:{id} Deleted SucessFully");
            }
        }

        [HttpPost]
        [Route("{internId}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<ApproveAndRegisterInternViewModel>>> ApproveAndRegister([FromRoute] int internId ,PaymentMethod paymentMethod )
        {
            var response = await mediator.Send(new ApproveandCreateInternOrchestratorCommand(internId,paymentMethod ));
            if (!response.Success || response.Data is null)
            {
                return EndpointResponse<ApproveAndRegisterInternViewModel>.Fail(response.StatusCode, $"Failed to Approve and Register the intern with Id:{internId}", response.Errors);
            }

            else
            {
                var vm = new ApproveAndRegisterInternViewModel()
                {
                    EnrollementId = response.Data.EnrollementId
                };
                return EndpointResponse<ApproveAndRegisterInternViewModel>.OK(vm,response.StatusCode, $"Intern With Id:{internId} Approved SucessFully");
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
