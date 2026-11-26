using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record CheckCompletedPaymentQuery(int enrollmentId) : IRequest<RequestResponse<Payment>>;

    public class CheckCompletedPaymentQueryHandler : IRequestHandler<CheckCompletedPaymentQuery,RequestResponse<Payment>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepo;

        public CheckCompletedPaymentQueryHandler(IGeneralRepository<Payment> paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<RequestResponse<Payment>> Handle(CheckCompletedPaymentQuery query,CancellationToken cancellationToken)
        {
            var payment = await _paymentRepo.GetTable().FirstOrDefaultAsync(p => p.EnrollmentId == query.enrollmentId);

            return RequestResponse<Payment>.OK(payment);
        }
    }
}
