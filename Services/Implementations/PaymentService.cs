using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Services.Implementations
{

    public class PaymentService : IPaymentService
    {
        private readonly IGeneralRepository<Payment> _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IGeneralRepository<Payment> paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            return await _paymentRepository
                .GetAll()
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    EnrollmentId = p.EnrollmentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Method = p.Method,
                    Status = p.Status
                })
                .ToListAsync();
        }

        public async Task<PaymentDto?> GetByEnrollmentAsync(int enrollmentId)
        {
            return await _paymentRepository
                .GetAll()
                .Where(p => p.EnrollmentId == enrollmentId)
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    EnrollmentId = p.EnrollmentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Method = p.Method,
                    Status = p.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto dto)
        {
            var entity = new Payment
            {
                EnrollmentId = dto.EnrollmentId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = dto.Method,
                Status = PaymentStatus.Pending
            };

            _paymentRepository.Add(entity);
            return new PaymentDto
            {
                Id = entity.Id,
                EnrollmentId = entity.EnrollmentId,
                Amount = entity.Amount,
                PaymentDate = entity.PaymentDate,
                Method = entity.Method,
                Status = entity.Status
            };
        }

        public async Task<bool> RefundPaymentAsync(int enrollmentId)
        {
            var payment = await _paymentRepository
                .GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId)
                ;

            if (payment == null) return false;

            payment.Status = PaymentStatus.Refunded;
            _paymentRepository.Update(payment);
            return true;
        }

        public async Task<bool> UpdatePaymentAmountAsync(int enrollmentId, decimal newAmount)
        {
            var payment = await _paymentRepository
                .GetAll()
                .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId)
                ;

            if (payment == null) return false;

            payment.Amount = newAmount;
            payment.Status = PaymentStatus.Pending;
            _paymentRepository.Update(payment);
            return true;
        }
    }
}
