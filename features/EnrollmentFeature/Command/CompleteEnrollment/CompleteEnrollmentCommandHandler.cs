using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Command.UpdateStatus;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Command.CompleteEnrollment;

public class CompleteEnrollmentCommandHandler(
    IGeneralRepository<Enrollment> enrollmentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CompleteEnrollmentCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(
        CompleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        // enrollment + its payment in one query
        var enrollment = await enrollmentRepository.GetAll()
            .Include(e => e.Payment)
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

        if (enrollment is null)
            return Fail(404, "EnrollmentId", "Enrollment not found");

        // only an Active enrollment can be completed
        if (enrollment.Status != EnrollmentStatus.Active)
            return Fail(409, "Status",
                $"Only an Active enrollment can be completed (current status: {enrollment.Status})");

        // must have a Completed payment
        if (enrollment.Payment is null)
            return Fail(409, "Payment", "Enrollment has no payment recorded");

        if (enrollment.Payment.Status != PaymentStatus.Completed)
            return Fail(409, "Payment",
                $"Enrollment payment must be Completed (current payment status: {enrollment.Payment.Status})");

        // change the enrollment status only
        enrollment.Status = EnrollmentStatus.Completed;
        await unitOfWork.SaveChangesAsync();

        return RequestResponse.OK();
    }

    private static RequestResponse Fail(int code, string key, string message)
        => RequestResponse.Fail(code, new Dictionary<string, string[]> { { key, [message] } });
}