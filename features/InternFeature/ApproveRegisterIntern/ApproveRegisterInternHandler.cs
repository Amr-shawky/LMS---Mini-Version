using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.EnrollmentFeature.ActiveEnrollment;
using LMS___Mini_Version.features.InternFeature.AcceptIntern;
using LMS___Mini_Version.features.PaymentFeature.CreatePayment;
using LMS___Mini_Version.features.shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.features.InternFeature.ApproveRegisterIntern
{
    public class ApproveRegisterInternHandler : IRequestHandler<ApproveRegisterInternOrchestrator, RequestResponse<ApproveRegisterInternDto>>
    {
        readonly private IUnitOfWork _unitOfWork;
        readonly private IMediator _mediator;
        public ApproveRegisterInternHandler(IUnitOfWork unitOfWork,IMediator mediator)
        {
            _unitOfWork = unitOfWork;
          
            _mediator = mediator;
        }
        public async Task<RequestResponse<ApproveRegisterInternDto>> Handle(ApproveRegisterInternOrchestrator request, CancellationToken cancellationToken)
        {
            
            RequestResponse<int> internResponse = await _mediator.Send(new AccepteInternCommand(request.InternId));
            if (!internResponse.Success)
                return RequestResponse<ApproveRegisterInternDto>.Fail(internResponse.StatusCode, errorCode: internResponse.ErrorCode);


            
           RequestResponse<int> enrollmentResponse = await _mediator.Send(new ActiveEnrollmentCommand(request.InternId, internResponse.Data));
            if (! enrollmentResponse.Success)
                return RequestResponse<ApproveRegisterInternDto>.Fail(enrollmentResponse.StatusCode, enrollmentResponse.ErrorCode);

            RequestResponse paymentResponse = await _mediator.Send(new CreatePaymentCommand(enrollmentResponse.Data, request.PaymentMethod));
            
            await _unitOfWork.SaveChangesAsync();
            return RequestResponse<ApproveRegisterInternDto>.OK(new ApproveRegisterInternDto
            {
                EnrollmentId = enrollmentResponse.Data,
               
            });
        }
  
    }
}
