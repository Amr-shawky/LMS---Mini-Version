using Azure.Core;
using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.CQRS.Enrollments.Query;
using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public class CancelEnrollmentOrchestratorHandler : IRequestHandler<CancelEnrollmentOrchestratorRequest, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        public CancelEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResult<bool>> Handle(CancelEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {
            //check if enroll is exist
            var Exist =await _mediator.Send(new GetByIdEnrollmentQuery(request.enrollmentId));
            if(!Exist.IsSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.EnrollMentNotExist);
            }

            //nchange status bt3 Enrollment eno keda cancelled
            var newStaus = EnrollmentStatus.Cancelled;
            var StatusUpdatedResult=await _mediator.Send(new UpdateStatusEnrollmentCommand(request.enrollmentId,newStaus));
            if (!StatusUpdatedResult.IsSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.UpdateError);
            }
            //
            var paymentStatus=PaymentStatus.Refunded;
            var UpdateResult =await _mediator.Send(new UpdatePaymentStatusCommand(request.enrollmentId, paymentStatus));

            if(!UpdateResult.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.paymentNotExist);

            return RequestResult<bool>.Success(true);



           
        }
    }
}
