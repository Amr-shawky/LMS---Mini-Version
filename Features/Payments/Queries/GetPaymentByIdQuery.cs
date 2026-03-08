using LMS___Mini_Version.ViewModels.Payment;
using MediatR;

namespace LMS___Mini_Version.Features.Payments.Queries
{
    /// <summary>
    /// [CQRS Assignment] Query to retrieve a single payment by its Id.
    /// </summary>
    public record GetPaymentByIdQuery(int Id) : IRequest<PaymentViewModel?>;
}
