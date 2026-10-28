using LMS___Mini_Version.Services.Interfaces;
using LMS___Mini_Version.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace LMS___Mini_Version.Controllers
{

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
            var dtos = await _paymentService.GetAllAsync();
            var viewModels = dtos.Select(d => new PaymentViewModel
            {
                Id = d.Id,
                EnrollmentId = d.EnrollmentId,
                Amount = d.Amount,
                PaymentDate = d.PaymentDate,
                Method = d.Method.ToString(),
                Status = d.Status.ToString()
            });
            return Ok(viewModels);
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<ActionResult<PaymentViewModel>> GetByEnrollment(int enrollmentId)
        {
            var dto = await _paymentService.GetByEnrollmentAsync(enrollmentId);
            if (dto == null) return NotFound();
            return Ok(new PaymentViewModel
            {
                Id = dto.Id,
                EnrollmentId = dto.EnrollmentId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                Method = dto.Method.ToString(),
                Status = dto.Status.ToString()
            });
        }
    }
}
