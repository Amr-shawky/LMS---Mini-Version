using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.ViewModels
{
    /// <summary>
    /// Request body. The intern id comes from the route.
    /// PaymentMethod accepts: Cash, CreditCard, BankTransfer, Online.
    /// </summary>
    public class ApproveAndRegisterInternRequest
    {
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
