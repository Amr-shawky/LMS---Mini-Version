using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class CancelEnrollmentCommandHandler(
    IEnrollmentService enrollmentService,
    IPaymentService paymentService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CancelEnrollmentCommand, bool>
{
    public async Task<bool> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {

        var enrollment = await enrollmentService.GetByIdAsync(request.Id);  
        if (enrollment == null)
        {
            return false; 
        }
        if(enrollment.Status == EnrollmentStatus.Cancelled)
        {
            return false;
        }
       var updated =await enrollmentService.UpdateStatusAsync(request.Id, EnrollmentStatus.Cancelled);

        if (!updated) return false; 
      
        await paymentService.RefundPaymentAsync(request.Id);

        await unitOfWork.CompleteAsync();
        return true;

    }
}
