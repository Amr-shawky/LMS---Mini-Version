using LMS___Mini_Version.Domain.Enums;
using System.Text.Json.Serialization;

namespace LMS___Mini_Version.features.InternFeature.ApproveAndRegisterIntern.ViewModels
{
    public class ApproveAndRegisterInternInputViewModel
    {
        [JsonRequired]
        public PaymentMethod Method { get; set; }
    }
}
