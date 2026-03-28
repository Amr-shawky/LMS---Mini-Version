using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.CQRS.Enrollments.Services
{
    public class EnrollmentCreator
    {
        private readonly IUnitOfWork _unitOf;
        public EnrollmentCreator(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<EnrollmentResultDto> CreateAsync(EnrollmentInternCommand command, Track track)
        {
            await using var transaction = await _unitOf.BeginTransactionAsync();

            try
            {
                var enrollment = new Enrollment()
                {
                    InternId = command.InternId,
                    TrackId = command.TrackId,
                    Status = EnrollmentStatus.Pending,
                    EnrollmentDate = DateTime.UtcNow
                };
                _unitOf.Enrollments.Add(enrollment);
                await _unitOf.CompleteAsync();

                Payment? payment = null;
                if (track.Fees > 0)
                {
                    payment = new Payment()
                    {
                        EnrollmentId = enrollment.Id,
                        Amount = track.Fees,
                        PaymentDate = DateTime.UtcNow,
                        Method = PaymentMethod.Cash,
                        Status = PaymentStatus.Pending
                    };
                    _unitOf.Payments.Add(payment);
                    await _unitOf.CompleteAsync();
                }

                await transaction.CommitAsync();
                return EnrollmentResultDto.Succeed(enrollment.ToDto(), payment?.ToDto());
            }
            // Handle database-specific errors, such as [ concurrency conflicts or constraint violations ]
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return EnrollmentResultDto.Fail("Concurrent enrollment detected. Please try again ");
            }
            // Catch-all block for any unexpected application errors (e.g., null references, network issues)
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

    }
}

