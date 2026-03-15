using LMS___Mini_Version.Features.Payments.Queries;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// [CQRS Fix] Read-only controller for Payment data.
    /// Injects ONLY IMediator — no more IPaymentService.
    /// Payments are created through the EnrollInternOrchestrator — not directly.
    /// All responses wrapped in EndpointResponse for a consistent API contract.
    /// </summary>
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
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            return Ok(EndpointResponse<IEnumerable<PaymentViewModel>>.SuccessResponse(result));
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<EndpointResponse<PaymentViewModel>>> GetByEnrollment(int enrollmentId)
        {
            var result = await _mediator
                .Send(new GetPaymentByEnrollmentQuery(enrollmentId));

            if (result == null)
                return NotFound(EndpointResponse<PaymentViewModel>.NotFoundResponse($"Payment for Enrollment ID {enrollmentId} was not found."));

            return Ok(EndpointResponse<PaymentViewModel>.SuccessResponse(result));
        }
    }
}
