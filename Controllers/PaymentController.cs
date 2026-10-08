using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.paymentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<IEnumerable<PaymentViewModel>>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllPaymentsQuery());
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, EndpointResponse<IEnumerable<PaymentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data!.Select(d => d.ToViewModel());
            return Ok(EndpointResponse<IEnumerable<PaymentViewModel>>.Ok(viewModels));
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<EndpointResponse<PaymentViewModel>>> GetByEnrollment(int enrollmentId)
        {
            var response = await _mediator.Send(new GetPaymentByEnrollmentQuery(enrollmentId));
            if (!response.Success || response.Data == null)
            {
                return NotFound(EndpointResponse<PaymentViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            return Ok(EndpointResponse<PaymentViewModel>.Ok(response.Data.ToViewModel()));
        }
    }
}
