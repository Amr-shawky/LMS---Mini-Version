using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.features.EnrollmentFeature.Command.CreateEnrollmentWithPayment;

public class CreateEnrollmentWithPaymentCommandHandler(
    IGeneralRepository<Enrollment> enrollmentRepository,
    IGeneralRepository<Payment> paymentRepository)
    : IRequestHandler<CreateEnrollmentWithPaymentCommand, Enrollment>
{
    public Task<Enrollment> Handle(CreateEnrollmentWithPaymentCommand request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var enrollment = new Enrollment
        {
            InternId = request.InternId,
            TrackId = request.TrackId,
            Status = EnrollmentStatus.Active,
            EnrollmentDate = now
        };

        var payment = new Payment
        {
            Enrollment = enrollment,
            Amount = request.Fees,
            Status = PaymentStatus.Completed,
            Method = request.Method,
            PaymentDate = now
        };

        enrollmentRepository.Add(enrollment);
        paymentRepository.Add(payment);

        return Task.FromResult(enrollment);
    }
}