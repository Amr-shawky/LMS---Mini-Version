using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollment.Commands;
using LMS___Mini_Version.Features.Enrollment.Queries;
using LMS___Mini_Version.Mediators;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.ControllerCQRS
{
    [ApiController]
    [Route("api/cqrs/[controller]")]
    public class EnrollmentCQRSController : ControllerBase
    {
        private readonly EnrollInternMediator _enrollInternMediator;
        private readonly IMediator _mediatoR;

        public EnrollmentCQRSController(IMediator mediator, EnrollInternMediator enrollInternMediator)
        {
            _mediatoR = mediator;
            _enrollInternMediator = enrollInternMediator;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediatoR.Send(new GetAllEnrollmentQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediatoR.Send(new GetByIdEnrollmentQuery(id));
            return Ok(result);

        }

        [HttpGet("intern/{internId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetByIntern(int internId)
        {
            var result = await _mediatoR.Send(new GetByInternIdQuery(internId));
            return Ok(result);
        }

        
        [HttpPost]
        public async Task<ActionResult<EnrollInternViewModel>> CreateEnroll(EnrollInternViewModel vm)
        {
         var result =await _mediatoR.Send(new CreateEnrollmentCommand(vm.InternId, vm.TrackId));
            return Ok(result);
        }



    }
}
