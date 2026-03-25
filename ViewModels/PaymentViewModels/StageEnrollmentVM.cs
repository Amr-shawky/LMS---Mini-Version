using LMS___Mini_Version.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LMS___Mini_Version.ViewModels.PaymentViewModels
{
    public class StageEnrollmentVM
    {
        [Required]
        public int InternId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    }
}
