using LMS___Mini_Version.Attributes;
using LMS___Mini_Version.features.InternFeature.AddIntern.Commands;
using LMS___Mini_Version.features.InternFeature.AddIntern.DTOS;
using LMS___Mini_Version.features.InternFeature.AddIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.Command;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.DTOS;
using LMS___Mini_Version.features.InternFeature.DeleteIntern.ViewModels;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.Query;
using LMS___Mini_Version.features.InternFeature.GetAllInterns.ViewModels;
using LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.DTOs;
using LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.Query;
using LMS___Mini_Version.features.InternFeature.InternRegistrationApproval.ViewModels;
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
        public async Task<ActionResult<EndpointResponse<IEnumerable<GetAllInternViewModel>>>> GetAll()
        {
            var result = await mediator.Send(new GetAllInternsQuery());
            if (result.Success)
            {
                var viewModel = result.Data!.Select(s => new GetAllInternViewModel(s.FullName, s.Email, s.Status, s.TrackName));
                return EndpointResponse<IEnumerable<GetAllInternViewModel>>.OK(viewModel, result.StatusCode, "sucess");

            }

            return EndpointResponse<IEnumerable<GetAllInternViewModel>>.Fail(result.StatusCode, "Failed to load the Interns ", result.Errors);

        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<>> GetById(int id)
        //{

        //}
        [SaveChanges]
        [HttpPost("Create")]
        public async Task<ActionResult<EndpointResponse>> Create([FromBody] CreateInternViewModel interViewModel)
        {
            var InternMap = new CreateInternDTO(
                 interViewModel.FullName,
                 interViewModel.Email,
                 interViewModel.TrackId,
                 interViewModel.BirthYear
                );
            var result = await mediator.Send(new AddInternCommand(InternMap));

            if (!result.Success)
                return EndpointResponse.Fail(result.StatusCode, "Failed to register intern", result.Errors);


            return EndpointResponse.OK(result.StatusCode, "Success");
        }
        [SaveChanges]
        [HttpPost("{internId}/approve-and-register")]
        public async Task<ActionResult<EndpointResponse>> RegisterApproval(int internId, InternRegistrationApprovalViewModel internRegistrationApprovalViewModel)
        {
            var dto = new InternRegistrationApprovalDTO(internRegistrationApprovalViewModel.PaymentMethod);
            var result = await mediator.Send(new InternRegistrationApprovalQuery(internId, dto));

            if (!result.Success)
                return EndpointResponse.Fail(result.StatusCode, "Failed to register intern", result.Errors);
            return EndpointResponse.OK(result.StatusCode, "Success");
        }
        [SaveChanges]
        [HttpDelete("{internId}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int internId)
        {
            var InternVm = new DeleteInternDTO(internId);
            var result = await mediator.Send(new DeleteInternCommand(InternVm));
            if (!result.Success)
                return EndpointResponse<DeleteInternViewModel>.Fail(result.StatusCode, "Error Occured",
                    result.Errors);
            return EndpointResponse.OK(result.StatusCode, "");
        }
    }
}
