using LMS___Mini_Version.Common;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Command;
using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Queries;
using LMS___Mini_Version.Features.Payment.RefunPayment.Command;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Orchestrator
{
    public record CancelEnrollmentOrchestrator(int enrollmentId) : IRequest<bool>;
    public class CancelEnrollmentOrchestratorHandler : IRequestHandler<CancelEnrollmentOrchestrator, bool>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public CancelEnrollmentOrchestratorHandler(IMediator mediator,IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            bool flag = true; 
            await _unitOfWork.ExecuteAsync(async () =>
            {
              var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentId));
                if(enrollment == null)
                {
                    flag = false;
                    return;
                    
                }
                if (enrollment.EnrollmentStatus != "Active")
                {
                    flag = false;
                    return;
                }
                if (enrollment.PaymentStatus == "Completed" )
                {
                  var result = await _mediator.Send(new RefundPaymentCommand(request.enrollmentId));
                    if (result == false)
                    {
                        flag = false;
                        return;
                    }
                }
                if (enrollment.EnrollmentStatus == "active")
                {
                    var result = await _mediator.Send(new CancelEnrollmentStatusCommand(request.enrollmentId));
                    if (result == false)
                    {
                        flag = false;
                        return;
                    }
                }
            });

            return flag;
        }
    }
}
