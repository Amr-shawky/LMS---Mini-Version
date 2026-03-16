using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Mediators;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Commands.CancelEnrollment;

public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand,EnrollmentResultDto>
{
    private readonly IUnitOfWork _uow;
    public CancelEnrollmentCommandHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<EnrollmentResultDto> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {
        // Check if the enrollment exists
        var enrollment = await _uow.Enrollments.GetByIdAsync(request.EnrollmentId).ConfigureAwait(false);
        if(enrollment == null) 
            throw new InvalidOperationException($"enrollment with id {request.EnrollmentId} doesn't exist");
        // Check if the enrollment is already canceled
        if(enrollment.Status == EnrollmentStatus.Cancelled)
            throw new InvalidOperationException($"enrollment with id {request.EnrollmentId} is already canceled");
        // start the transaction (cancel enrollment & refund the payment)
        await using var transaction = await _uow.BeginTransactionAsync().ConfigureAwait(false);

        try
        {
            enrollment.Status = EnrollmentStatus.Cancelled;
          _uow.Enrollments.Update(enrollment);
           
          // Refund the payment
          Payment? payment = null;
          if (enrollment.Payment != null)
          {
              var refunded = await _uow.Payments.RefundPaymentAsync(request.EnrollmentId).ConfigureAwait(false);
              if(refunded) 
                  payment = enrollment.Payment;
          }
          await _uow.CompleteAsync().ConfigureAwait(false);
          await transaction.CommitAsync().ConfigureAwait(false);
          return EnrollmentResultDto.Succeed(
              enrollment.ToDto(),
              payment?.ToDto());

        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }

    }
}