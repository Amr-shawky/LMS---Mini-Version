using LMS___Mini_Version.features.EnrollmentFeatures.GetEnrollments.Handler;
using LMS___Mini_Version.features.InternFeatures.ApproveIntern.Orchestrator;
using LMS___Mini_Version.features.InternFeatures.ApproveInternRegistration.Command;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Dtos;
using LMS___Mini_Version.features.InternFeatures.GetAllInterns.Query;
using LMS___Mini_Version.features.InternFeatures.GetInternById.Query;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LMS___Mini_Version.features.InternFeatures.ApproveIntern.Handler
{
    public class ApproveInternOrchestratorHandler(IMediator mediator) : IRequestHandler<ApproveInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator mediator = mediator;

        public async Task<RequestResponse<int>> Handle(ApproveInternOrchestrator request, CancellationToken cancellationToken)
        {
            var Intern = await mediator.Send(new GetInternByIdQuery(request.InternId));
            if (!Intern.Success)
            {
                return RequestResponse<int>.Fail(Intern.StatusCode, Intern.Errors);
            }

            var Track = await mediator.Send(new GetTrackByIdQuery(Intern.Data!.TrackId));
            if (!Track.Success)
            {
                return RequestResponse<int>.Fail(Track.StatusCode, Track.Errors);

            }

            var Approval = await mediator.Send(new ApproveAndRegisterInternCommand(request.InternId, request.PaymentMethod));
            if (!Approval.Success)
            {
                return RequestResponse<int>.Fail(Approval.StatusCode, Approval.Errors);
            }
            return Approval;
        }
    }
}
