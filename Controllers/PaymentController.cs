using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Payment;
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
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> GetAll()
        {
            var dtos = await _paymentService.GetAllAsync().ConfigureAwait(false);
            var viewModels = dtos.Select(d => d.ToViewModel());
            return Ok(viewModels);
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<PaymentViewModel>> GetByEnrollment(int enrollmentId)
        {
            var dto = await _paymentService.GetByEnrollmentAsync(enrollmentId).ConfigureAwait(false);
            if (dto == null) return NotFound();
            return Ok(dto.ToViewModel());
        }
    }
}
