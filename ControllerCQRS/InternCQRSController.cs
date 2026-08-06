using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.ControllerCQRS
{
    [ApiController]
    [Route("api/cqrs/[controller]")]
    public class InternCQRSController : ControllerBase
    {

        private readonly IMediator _mediator;

        public InternCQRSController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternDetailViewModel>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllInternsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<InternSummaryViewModel>> GetById(int id)
        {
            var result = await _mediator.Send(new GetByIdInternQuery(id));
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<InternSummaryViewModel>> CreateIntern([FromBody] InternDto dto)
        {
            var result = await _mediator.Send(new CreateInternCommand(dto));

            return Ok(result);
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<Unit>> UpdateIntern(int id, [FromBody] UpdateInternViewModel vm)
        {
            var result = await _mediator.Send(new UpdateInternByIdCommand(id, vm));
            return Ok(result);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeleteIntern(int id)
        {
            var result = await _mediator.Send(new DeleteInternByIdCommand(id));
            return Ok(result);  
        }
    }
}
