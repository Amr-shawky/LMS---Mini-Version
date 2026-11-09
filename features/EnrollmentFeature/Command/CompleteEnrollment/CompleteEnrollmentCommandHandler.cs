using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.Command.UpdateStatus;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.EnrollmentFeature.Command.CompleteEnrollment;

public class CompleteEnrollmentCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository,
    IGeneralRepository<Payment> paymentRepository , IUnitOfWork unitOfWork)
    : IRequestHandler<CompleteEnrollmentCommand , RequestResponse>
{
    public async Task<RequestResponse> Handle(CompleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        // get enrollment
        var enrollment = await enrollmentRepository.GetByIdAsync(request.EnrollmentId);
        if (enrollment is null)
            return RequestResponse.Fail(404, new Dictionary<string, string[]>
            {
                { "EnrollmentId", ["Enrollment not found"] }
            });
        
        // only Active enrollment can be completed
        if (enrollment.Status != EnrollmentStatus.Active)
            return RequestResponse.Fail(400, new Dictionary<string, string[]>
            {
                { "Status", ["Only an Active enrollment can be completed"] }
            });
        
        // must have a Completed payment
        var hasCompletedPayment = await paymentRepository.GetAll()
            .AnyAsync(p => p.EnrollmentId == request.EnrollmentId
                           && p.Status == PaymentStatus.Completed, cancellationToken);

        if (!hasCompletedPayment)
            return RequestResponse.Fail(400, new Dictionary<string, string[]>
            {
                { "Payment", ["Enrollment must have a Completed payment"] }
            });
        
        
        // change enrollment status only
        enrollment.Status = EnrollmentStatus.Completed;
        await unitOfWork.SaveChangesAsync();

        return RequestResponse.OK();
        
        
        
    }
}