using LMS___Mini_Version.CQRS.Enrollments.Orchestrator;
using LMS___Mini_Version.ViewModels.PaymentViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController:ControllerBase
    {
        private readonly IMediator _mediator;
        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> StageNewEnrollmet(StageEnrollmentVM vM)
        {
           var EnrollmentResult=await _mediator.Send(new EnrollInternOrchestratorRequest(vM.InternId, vM.PaymentMethod));
            // Return just success message
            return  Ok(new { message = "Enrollment successful" });
        }
    }
}
