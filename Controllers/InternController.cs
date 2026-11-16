using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.InternFeatures.ApproveAndRegisterIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.ApproveAndRegisterIntern.ViewModels;
using LMS___Mini_Version.features.InternFeatures.CreateIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.CreateIntern.ViewModels;
using LMS___Mini_Version.features.InternFeatures.DeleteIntern.Commands;
using LMS___Mini_Version.features.InternFeatures.GetallIntern.DTOs;
using LMS___Mini_Version.features.InternFeatures.GetallIntern.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace LMS___Mini_Version.Controllers;


[ApiController]
[Route("api/[controller]")]
public class InternController(IMediator mediator) : ControllerBase
{



    [HttpGet]
    public async Task<ActionResult<EndpointResponse<IEnumerable<InternListVM>>>> GetAll()
    {
        var result = await mediator.Send(new GetallInternQuery());

        var internDto = result.Data!.Select(x=> new InternListVM
        {
            FullName = x.FullName,
            Email = x.Email,
            Status = x.Status,
            Id = x.Id,
            TrackName = x.TrackName,
        }); 

        var response = EndpointResponse<IEnumerable<InternListVM>>.OK(internDto, result.StatusCode,
            result.Success ? "Geting all Interns Successfully" : "Error");

        return StatusCode(result.StatusCode, response);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<>> GetById(int id)
    //{

    //}

    [HttpPost]
    public async Task<ActionResult<EndpointResponse<int>>> Create([FromBody]CreateInternVM vm)
    {
       var result = await mediator.Send(new CreateInternCommand(vm.FullName, vm.Email,vm.BirthYear,vm.TrackId));

        var response = EndpointResponse<int>.OK(result.Data,201,
                 result.Success ? "Intern  created successfully" : "Failed to create Intern"
             );
        return StatusCode(200,response);

    }

    [HttpPost("{internId:int}/approve-and-register")]
    public async Task<ActionResult<EndpointResponse<int>>> ApproveAndRegister(
    int internId, [FromBody] ApproveAndRegisterViewModel vm)
    {
        var result = await mediator.Send(
            new ApproveAndRegisterInternCommand(internId, vm.PaymentMethod));

        EndpointResponse<int> response = result.Success
            ? EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern approved and registered successfully.")
            : EndpointResponse<int>.Fail(result.StatusCode, "Failed to approve and register intern.", result.Errors);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EndpointResponse>> Delete(int id)
    {
        var result = await mediator.Send(new DeleteInternCommand(id));

        var response = EndpointResponse.OK(201,
                 result.Success ? "Intern  Deleted successfully" : "Failed to delete Intern"
             );
        return StatusCode(200, response);

    }
}
