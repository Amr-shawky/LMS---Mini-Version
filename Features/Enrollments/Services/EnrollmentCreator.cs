using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Mediators;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Services;

public class EnrollmentCreator
{
    private readonly IUnitOfWork _uow;
    public EnrollmentCreator(IUnitOfWork uow) => _uow = uow;

    public async Task<EnrollmentResultDto> CreateAsync(EnrollmentInternCommand command,Track track)
    {
        await using var transaction = await _uow.BeginTransactionAsync().ConfigureAwait(false);

        try
        {
            var enrollment = new Enrollment
            {
                InternId = command.InternId,
                TrackId = command.TrackId,
                Status = EnrollmentStatus.Pending,
                EnrollmentDate = DateTime.UtcNow
            };
            _uow.Enrollments.Add(enrollment);
            await _uow.CompleteAsync().ConfigureAwait(false);
            Payment? payment = null;
            if (track.Fees > 0)
            {
                payment = new Payment
                {
                    EnrollmentId = enrollment.Id,
                    Amount = track.Fees,
                    Method = PaymentMethod.Cash,
                    Status = PaymentStatus.Pending
                };
                _uow.Payments.Add(payment);
                await _uow.CompleteAsync().ConfigureAwait(false);
                
            }

            await transaction.CommitAsync().ConfigureAwait(false);
            return EnrollmentResultDto.Succeed(
                enrollment.ToDto(),
                payment?.ToDto());
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return EnrollmentResultDto.Fail("Concurrent enrollment detected. Please try again ");
        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }
    }
}