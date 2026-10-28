using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.ViewModels.Payment
{

    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
