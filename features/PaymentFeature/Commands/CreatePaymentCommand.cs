using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.PaymentFeature.Commands
{
    public record CreatePaymentCommand (int enrollementId , decimal Amount , PaymentStatus PaymentStatus  , PaymentMethod PaymentMethod , DateTime date  ): IRequest<RequestResponse>;
    }
