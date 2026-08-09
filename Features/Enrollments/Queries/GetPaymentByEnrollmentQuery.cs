using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Queries
{
    public record GetPaymentByEnrollmentQuery(int EnrollmentId) : IRequest<PaymentDto?>;


    public class GetPaymentByEnrollmentQueryHandler : IRequestHandler<GetPaymentByEnrollmentQuery, PaymentDto?>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetPaymentByEnrollmentQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto?> Handle(GetPaymentByEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository
                .GetTable()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentId, cancellationToken);

            return payment?.ToDto();
        }
    }
}
