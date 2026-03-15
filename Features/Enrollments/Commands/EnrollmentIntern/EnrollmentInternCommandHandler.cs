using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.Mediators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands.EnrollmentIntern;

public class EnrollmentInternCommandHandler:IRequestHandler<EnrollmentIntern.EnrollmentInternCommand,EnrollmentResultDto>
{
    private readonly UnitOfWork _uow;
    public EnrollmentInternCommandHandler(UnitOfWork unitofwork) => _uow = unitofwork;
    public async Task<EnrollmentResultDto> Handle(EnrollmentIntern.EnrollmentInternCommand request, CancellationToken cancellationToken)
    {
        // Check if the intern and track exists
        var intern = await _uow.Interns.GetByIdAsync(request.InternId).ConfigureAwait(false);
        if (intern == null) return EnrollmentResultDto.Fail($"Intern with Id {request.InternId} doesn't exist");
        // Check if the track is active
        var track = await _uow.Tracks.GetByIdAsync(request.TrackId).ConfigureAwait(false);
        if (track == null) return EnrollmentResultDto.Fail($"Track with Id {request.TrackId} doesn't exist");
        // Check if the track has capacity
        var hasCapacity = await _uow.Tracks.CheckCapacityAsync(request.TrackId).ConfigureAwait(false);
        if (!hasCapacity) return EnrollmentResultDto.Fail($"Track with Id{request.TrackId} has no capacity!");
        // Check Duplicate 
        var isDuplicate = await _uow.Enrollments.HasActiveEnrollmentAsync(request.InternId,request.TrackId).ConfigureAwait(false);
        if (!isDuplicate)
            return  EnrollmentResultDto.Fail($"Intern with the Id{request.InternId} has already active enrollment");
        
        
        // Start the transaction 
        await using var transaction = await _uow.BeginTransactionAsync().ConfigureAwait(false);

        try
        {
            var enrollment = new Enrollment
            {
                InternId = request.InternId,
                TrackId = request.TrackId,
                Status = EnrollmentStatus.Pending,
                EnrollmentDate = DateTime.UtcNow
            };
            _uow.Enrollments.Add(enrollment);
            await _uow.CompleteAsync().ConfigureAwait(false);

            // Create the payment with real EnrollmentId
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