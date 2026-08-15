using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Services.Interfaces;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand>
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;
    public CancelEnrollmentCommandHandler(IEnrollmentService enrollmentService, IPaymentService paymentService, IUnitOfWork unitOfWork)
    {
        _enrollmentService = enrollmentService;
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {
        await _enrollmentService.UpdateStatusAsync(request.EnrollmentId, EnrollmentStatus.Cancelled);
        await _paymentService.RefundPaymentAsync(request.EnrollmentId);
        await _unitOfWork.CompleteAsync();
        
    }
    
}
