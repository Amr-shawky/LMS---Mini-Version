using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeature.Command.ApproveRegistration;
using LMS___Mini_Version.features.InternFeature.Command.Create;
using LMS___Mini_Version.features.InternFeature.Command.DeleteIntern;
using LMS___Mini_Version.features.InternFeature.Queries.GetAll;
using LMS___Mini_Version.features.InternFeature.ViewModels;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<List<InternViewModel>>>> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllInternsQuery(), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<List<InternViewModel>>.Fail(result.StatusCode, "Failed to get interns", result.Errors));

            var interns = (result.Data ?? []).Select(x => new InternViewModel(
                x.Id, x.FullName, x.Email, x.Status, x.TrackName)).ToList();

            return StatusCode(result.StatusCode,
                EndpointResponse<List<InternViewModel>>.OK(interns, result.StatusCode));
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{
           
        //}

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<int>>> Create(
            [FromBody] CreateInternViewModel model, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CreateInternCommand(model.FullName, model.Email, model.BirthYear, model.TrackId),
                cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<int>.Fail(result.StatusCode, "Failed to create intern", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern created successfully"));
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update()
        //{
           
        //}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteInternCommand(id), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse.Fail(result.StatusCode, "Failed to delete intern", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse.OK(result.StatusCode, "Intern deleted successfully"));
        }

        
      

        [HttpPost("{internId:int}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse<int>>> ApproveAndRegister(
            int internId, [FromBody] ApproveInternViewModel model, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new ApproveAndRegisterInternCommand(internId, model.PaymentMethod), cancellationToken);

            if (!result.Success)
                return StatusCode(result.StatusCode,
                    EndpointResponse<int>.Fail(result.StatusCode, "Failed to approve intern", result.Errors));

            return StatusCode(result.StatusCode,
                EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern approved and registered successfully"));
        }
    }
}
