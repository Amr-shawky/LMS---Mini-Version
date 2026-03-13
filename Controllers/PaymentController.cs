using LMS___Mini_Version.CQRS.Payments.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{
    /// <summary>
    /// Read-only controller for Payment data.
    /// Payments are created through the EnrollInternMediator — not directly.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> GetAll()
        {
            //var dtos = await _paymentService.GetAllAsync().ConfigureAwait(false);

            var dtos = await _mediator.Send(new GetAllPaymentQuery()).ConfigureAwait(false);
            var viewModels = dtos.Select(d => d.ToViewModel());
            return Ok(viewModels);
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<PaymentViewModel>> GetByEnrollment(int enrollmentId)
        {
            //var dto = await _paymentService.GetByEnrollmentAsync(enrollmentId).ConfigureAwait(false);

            var dto = await _mediator.Send(new GetPaymentByEnrollmentIdQuery(enrollmentId)).ConfigureAwait(false);
            if (dto == null) return NotFound();
            return Ok(dto.ToViewModel());
        }
    }
}
