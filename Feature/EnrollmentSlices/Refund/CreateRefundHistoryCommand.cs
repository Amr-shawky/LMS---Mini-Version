using Microsoft.EntityFrameworkCore;
using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Commands;

public record CreateRefundHistoryCommand(int PaymentId, int EnrollmentId) : IRequest<RequestResponse<int>>;

public sealed class CreateRefundHistoryCommandHandler(IGeneralRepository<RefundHistory> repository)
    : IRequestHandler<CreateRefundHistoryCommand, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(CreateRefundHistoryCommand request, CancellationToken ct)
    {
        if (request.PaymentId <= 0 || request.EnrollmentId <= 0)
            return RequestResponse<int>.Fail("Invalid refund history reference.");
        if (await repository.GetAll().AnyAsync(h => h.PaymentId == request.PaymentId && h.NewStatus == PaymentStatus.Refunded, ct))
            return RequestResponse<int>.Fail("Refund history already exists.", 409);

        var history = new RefundHistory {
            PaymentId = request.PaymentId, EnrollmentId = request.EnrollmentId,
            PreviousStatus = PaymentStatus.Completed, NewStatus = PaymentStatus.Refunded,
            CreatedAt = DateTime.UtcNow
        };
        repository.Add(history);
        await repository.SaveChangesAsync();
        return RequestResponse<int>.Created(history.Id);
    }
}
