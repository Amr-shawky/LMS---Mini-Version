using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Payments.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;

namespace LMS___Mini_Version.Features.Payments.Handlers
{
    /// <summary>
    /// [CQRS Assignment] Handler for GetPaymentByIdQuery.
    /// YOUR TASK: Query the database for a payment with the matching Id,
    /// map it to PaymentViewModel, and return it (or null if not found).
    /// 
    /// HINTS:
    ///   - Use _paymentRepository.GetByIdAsync(request.Id)
    ///   - Map using .ToDto().ToViewModel()
    ///   - Return null if payment is not found
    /// </summary>
    public class GetPaymentByIdQueryHandler
        : IRequestHandler<GetPaymentByIdQuery, PaymentViewModel?>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetPaymentByIdQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentViewModel?> Handle(
            GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            // ╔══════════════════════════════════════════════════════════════╗
            // ║  🎯 ASSIGNMENT: Implement this handler                      ║
            // ║                                                              ║
            // ║  Find a Payment by request.Id                               ║
            // ║  Map Payment entity → PaymentDto → PaymentViewModel         ║
            // ║  Return null if not found                                    ║
            // ╚══════════════════════════════════════════════════════════════╝
            throw new NotImplementedException("TODO: Implement this handler for the CQRS assignment");
        }
    }
}
