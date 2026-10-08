using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.paymentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.paymentFeature.Handlers
{
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, RequestResponse<IEnumerable<PaymentDto>>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetAllPaymentsQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<IEnumerable<PaymentDto>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetAll()
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    EnrollmentId = p.EnrollmentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Method = p.Method,
                    Status = p.Status
                }).ToListAsync(cancellationToken);

            return RequestResponse<IEnumerable<PaymentDto>>.Ok(payments);
        }
    }
}
