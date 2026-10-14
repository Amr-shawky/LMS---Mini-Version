using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.paymentFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.paymentFeature.Handlers
{
    public class GetPaymentByEnrollmentQueryHandler : IRequestHandler<GetPaymentByEnrollmentQuery, RequestResponse<PaymentDto>>
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;

        public GetPaymentByEnrollmentQueryHandler(IGeneralRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RequestResponse<PaymentDto>> Handle(GetPaymentByEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == request.EnrollmentId, cancellationToken);

            if (payment == null)
            {
                return RequestResponse<PaymentDto>.Fail($"Payment for enrollment ID {request.EnrollmentId} not found", 404);
            }

            var dto = new PaymentDto
            {
                Id = payment.Id,
                EnrollmentId = payment.EnrollmentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Method = payment.Method,
                Status = payment.Status
            };

            return RequestResponse<PaymentDto>.Ok(dto);
        }
    }
}
