using LMS___Mini_Version.Features.Payments.Queries;
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
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            return Ok(result);
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<PaymentViewModel>> GetByEnrollment(int enrollmentId)
        {
            var result = await _mediator
                .Send(new GetPaymentByEnrollmentQuery(enrollmentId));

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 6: GetPaymentByIdQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Payments/Queries/
            // 2) Create the Handler class in Features/Payments/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            throw new NotImplementedException("Task 6: Wire this endpoint using IMediator");
        }

        [HttpGet("pending")]
        public async Task<ActionResult> GetPending()
        {
            // ══════════════════════════════════════════════════════════════
            // 🎯 CQRS ASSIGNMENT — Task 7: GetPendingPaymentsQuery
            // ══════════════════════════════════════════════════════════════
            // TODO: The service method has been removed.
            // 1) Create the Query record class in Features/Payments/Queries/
            // 2) Create the Handler class in Features/Payments/Handlers/
            // 3) Use _mediator.Send(...) here to dispatch the query
            //    and return the result
            // ══════════════════════════════════════════════════════════════
            throw new NotImplementedException("Task 7: Wire this endpoint using IMediator");
        }
    }
}
