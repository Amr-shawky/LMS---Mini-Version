using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Features.Interns.DTOs;
using LMS___Mini_Version.Features;
using LMS___Mini_Version.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Shared.LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator Mediator;

        public InternController(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllinternsDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var Interns = await Mediator.Send(new GetAllInternsQuery(), cancellationToken);
            return Ok(Interns);
        }

        //[HttpGet("{id}")]
        //public ActionResult<Intern> GetById(int id)
        //{
        //    var intern = _context.Interns.Find(id);

        //    if (intern == null) return NotFound();

        //    return intern;
        //}


        //[HttpPut("{id}")]
        //public ActionResult Update(int id, Intern updatedIntern)
        //{
        //    var intern = _context.Interns.Find(id);
        //    if (intern == null) return NotFound();

        //    intern.FullName = updatedIntern.FullName;
        //    intern.Email = updatedIntern.Email;
        //    intern.BirthYear = updatedIntern.BirthYear;
        //    intern.Status = updatedIntern.Status;
        //    intern.TrackId = updatedIntern.TrackId;

        //    _context.SaveChanges();
        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndpointResponse>> Delete(int id,CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteUnsendInternRecordCommand(id), cancellationToken);
            if (result.StatusCode==404)
                return new EndpointResponse
                {
                    StatusCode = 404,
                    Message = "intern is not found",
                    Success = false,
                    TimeStamp = DateTime.UtcNow,
                    Errors = result.Errors
                };
            if (result.StatusCode == 409)
                return new EndpointResponse
                {
                    StatusCode = 409,
                    Message = "Can't delete the intern cause it has enrollments",
                    Success = false,
                    TimeStamp = DateTime.UtcNow,
                    Errors = result.Errors
                };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateInternCommand command,CancellationToken cancellationToken)
        {
            var InternId = await Mediator.Send(command, cancellationToken);

            return Ok(InternId);

        }
        [HttpPost("{internId}/approve-and-register")]
        public async Task<EndpointResponse> ApproveRegisteration(int internId,string paymentMethod,CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new ApproveAndCompleteInternRegistrationCommand(internId, paymentMethod), cancellationToken);
            if (!result.Success)
            {
                return new EndpointResponse
                {
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Success = false,
                    TimeStamp = DateTime.UtcNow,
                };
            }
            else
                return new EndpointResponse
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Registeration Completed Successfully"
                };
        }
    }
}