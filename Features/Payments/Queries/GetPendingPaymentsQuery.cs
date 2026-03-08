using LMS___Mini_Version.ViewModels.Payment;
using MediatR;

namespace LMS___Mini_Version.Features.Payments.Queries
{
    /// <summary>
    /// [CQRS Assignment] Query to retrieve all payments with a Pending status.
    /// </summary>
    public record GetPendingPaymentsQuery : IRequest<IEnumerable<PaymentViewModel>>;
}
