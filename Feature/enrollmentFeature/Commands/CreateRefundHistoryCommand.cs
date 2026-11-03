using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands
{
    public record CreateRefundHistoryCommand(int PaymentId, int EnrollmentId, PaymentStatus PreviousStatus)
        : IRequest<RequestResponse<bool>>;

    public sealed class CreateRefundHistoryCommandHandler
        : IRequestHandler<CreateRefundHistoryCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<RefundHistory> _repository;

        public CreateRefundHistoryCommandHandler(IGeneralRepository<RefundHistory> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResponse<bool>> Handle(CreateRefundHistoryCommand request, CancellationToken cancellationToken)
        {
            _repository.Add(new RefundHistory
            {
                PaymentId = request.PaymentId,
                EnrollmentId = request.EnrollmentId,
                PreviousStatus = request.PreviousStatus,
                NewStatus = PaymentStatus.Refunded,
                CreatedAt = DateTime.UtcNow
            });
            await _repository.SaveChangesAsync();
            return RequestResponse<bool>.Ok(true);
        }
    }
}