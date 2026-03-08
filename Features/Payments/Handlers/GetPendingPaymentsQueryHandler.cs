using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Payments.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Payment;
using MediatR;

namespace LMS___Mini_Version.Features.Payments.Handlers
{
    /// <summary>
    /// [CQRS Assignment] Handler for GetPendingPaymentsQuery.
    /// YOUR TASK: Query the database for all payments where Status == PaymentStatus.Pending,
    /// map them to PaymentViewModel, and return the list.
    /// 
    /// HINTS:
    ///   - Use _paymentRepository.GetTable() to get IQueryable
    ///   - Filter with .Where(p => p.Status == PaymentStatus.Pending)
    ///   - Include the Enrollment navigation if needed
    ///   - Use .ToListAsync(cancellationToken)
    ///   - Map using .ToDto().ToViewModel()
    /// </summary>
    public class GetPendingPaymentsQueryHandler
        : IRequestHandler<GetPendingPaymentsQuery, IEnumerable<PaymentViewModel>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetPendingPaymentsQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentViewModel>> Handle(
            GetPendingPaymentsQuery request, CancellationToken cancellationToken)
        {
            // ╔══════════════════════════════════════════════════════════════╗
            // ║  🎯 ASSIGNMENT: Implement this handler                      ║
            // ║                                                              ║
            // ║  Return all payments where Status == PaymentStatus.Pending   ║
            // ║  Map each Payment entity → PaymentDto → PaymentViewModel    ║
            // ╚══════════════════════════════════════════════════════════════╝
            throw new NotImplementedException("TODO: Implement this handler for the CQRS assignment");
        }
    }
}
