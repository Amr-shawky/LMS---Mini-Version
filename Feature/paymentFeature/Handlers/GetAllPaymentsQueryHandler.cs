using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.paymentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.paymentFeature.Handlers
{
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, RequestResponse<PaginatedResult<PaymentDto>>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetAllPaymentsQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<PaginatedResult<PaymentDto>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentRepository.GetAll();

            var payments = await query
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Take(request.PageSize)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    EnrollmentId = p.EnrollmentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Method = p.Method,
                    Status = p.Status
                }).ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            var paginatedResult = PaginatedResult<PaymentDto>.Create(
                payments,
                totalCount,
                request.PageIndex,
                request.PageSize
            );

            return RequestResponse<PaginatedResult<PaymentDto>>.Ok(paginatedResult);
        }
    }
}
