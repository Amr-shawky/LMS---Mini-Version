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
        public async Task<ActionResult<EndpointResponse<PaginatedResult<PaymentViewModel>>>> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetAllPaymentsQuery(pageIndex, pageSize));
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.StatusCode, EndpointResponse<PaginatedResult<PaymentViewModel>>.Fail(response.Message, response.StatusCode, response.Errors));
            }

            var viewModels = response.Data.Items.Select(d => d.ToViewModel()).ToList();
            var paginated = PaginatedResult<PaymentViewModel>.Create(viewModels, response.Data.TotalCount, response.Data.PageIndex, response.Data.PageSize);
            return Ok(EndpointResponse<PaginatedResult<PaymentViewModel>>.Ok(paginated));
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
