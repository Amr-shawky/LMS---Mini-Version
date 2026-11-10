using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.features.EnrollementFeature.Commands;
using LMS___Mini_Version.features.EnrollementFeature.Query;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.Commands;
using LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.DTOs;
using LMS___Mini_Version.features.PaymentFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.CheckTrackAvaiability.Query;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.Orchestrators.ApproveAndCreateInternOrchestrator.Handlers
{
    public class ApproveAndCreateInternOrchestratorHandler :IRequestHandler<ApproveandCreateInternOrchestratorCommand,RequestResponse<ApproveAndRegisterDTO>>
    {
        private readonly IMediator mediator;

        public ApproveAndCreateInternOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        
        public async Task<RequestResponse<ApproveAndRegisterDTO>> Handle(ApproveandCreateInternOrchestratorCommand request, CancellationToken cancellationToken)
        {

            // get intern
            var internResponse = await mediator.Send(new GetInternByIdQuery(request.internId));
            if(!internResponse.Success || internResponse.Data is null)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(internResponse.StatusCode, internResponse.Errors);

            }
            var internData = internResponse.Data;
            // get status
            if (internData.Status != "Pending")
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(400, new Dictionary<string, string[]>
                {
                    { "Status", new[] { "Only Pending interns can be approved." } }
                });
            }

            // get track 
            var trackResponse = await mediator.Send(new GetTrackByIdQuery(internData.TrackId));
            if(!trackResponse.Success )
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(trackResponse.StatusCode, trackResponse.Errors);
            }
            // track exists and active
            var Track = trackResponse.Data;
            if(Track == null || !Track.IsActive)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(400, new Dictionary<string, string[]>
                {
                    { "Track", new[] { "The Track Doesnot Exist or not Active." } }
                });

            }
            // check Avaiability
            var avaiabilityResponse = await mediator.Send(new CheckTrackAvaiabilityQuery(Track.Id));
            if(!avaiabilityResponse.Success)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(avaiabilityResponse.StatusCode, avaiabilityResponse.Errors);
            }
            if(avaiabilityResponse.Data!.AvailableSeats <=0)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(400, new Dictionary<string, string[]>
                {
                    { "Track Avaiability", new[] { "The Track Doesnot have Enough Seats. " } }
                });
            }
            // enrollement with track id and internId
            var enrollementResponse = await mediator.Send(new GetEnrollementWithInternIdQuery(internData.Id));
            if(!enrollementResponse.Success || enrollementResponse.Data is null)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(enrollementResponse.StatusCode, enrollementResponse.Errors);
            }
            bool exist = enrollementResponse.Data.Any(x => x.TrackId == internData.TrackId && (x.Status == Domain.Enums.EnrollmentStatus.Pending || x.Status == Domain.Enums.EnrollmentStatus.Active));
            if(exist)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(400, new Dictionary<string, string[]>
                {
                    { "Enrollement", new[] { "This intern already has Pending or Active Enrollement. " } }
                });

            }

            var approveResponse = await mediator.Send(new ApproveInternCommand(internData.Id));
            if(!approveResponse.Success)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(approveResponse.StatusCode, approveResponse.Errors);

            }
            // create enrollment
            var createEnrollementResponse = await  mediator.Send(new CreateEnrollementCommand( internData.Id , internData.TrackId, Domain.Enums.EnrollmentStatus.Active, DateTime.Now)
                
                );
            if(!createEnrollementResponse.Success)
            {
                return RequestResponse<ApproveAndRegisterDTO>.Fail(createEnrollementResponse.StatusCode, createEnrollementResponse.Errors);
            }
            //create payment
            var enrollId = createEnrollementResponse.Data!.Id;
            var createPaymentResponse = await mediator.Send(new CreatePaymentCommand(enrollId, Track.Fees, Domain.Enums.PaymentStatus.Completed, request.paymentMethod, DateTime.Now));

            if (!createPaymentResponse.Success)
                return RequestResponse<ApproveAndRegisterDTO>.Fail(
                    createPaymentResponse.StatusCode,
                    createPaymentResponse.Errors);

            return RequestResponse<ApproveAndRegisterDTO>.OK(
            new ApproveAndRegisterDTO
            {
                EnrollementId = enrollId,
            },
            200);


        }
    }
}
