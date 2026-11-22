using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.features.EnrollmentFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.PaymentFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.shared.PipelineBehavior;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using LMS___Mini_Version.features.TrackFeature.Orchestrators;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Orchestrators
{
    public record ApproveAndRegisterInternOrchestrator(int InternId, PaymentMethod Method): IRequest<RequestResponse<int>>, ITransactionalRequest;

    public class ApproveAndRegisterInternOrchestratorHandler : IRequestHandler<ApproveAndRegisterInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator _mediator;

        public ApproveAndRegisterInternOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<int>> Handle(ApproveAndRegisterInternOrchestrator request, CancellationToken ct)
        {
            if(request.InternId <= 0)
                return RequestResponse<int>.Fail(ErrorCode.InvalidInternId);

            var intern = await _mediator.Send(new GetInternByIdQuery(request.InternId), ct);
            if (!intern.Success) 
                return RequestResponse<int>.Fail(ErrorCode.InvalidInternId);
            if (intern.Data.Status != "Pending")
                return RequestResponse<int>.Fail(ErrorCode.InternWithNotPendingStatus);

            var track = await _mediator.Send(new GetTrackByIdQuery(intern.Data.TrackId), ct);
            if (!track.Success) 
                return RequestResponse<int>.Fail(ErrorCode.TrackNotFound);
            if (!track.Data.IsActive)
                return RequestResponse<int>.Fail(ErrorCode.TrackNotActive);

            var availability = await _mediator.Send(new TrackAvailabilityOrchestrator(track.Data.Id), ct);
            if (!availability.Success) 
                return RequestResponse<int>.Fail(ErrorCode.InvalidTrackId);
            if (availability.Data.AvailabeSeats <= 0)
                return RequestResponse<int>.Fail(ErrorCode.TrackHasNoAvailableSeats);

            var enrollment = await _mediator.Send(new CreateActiveEnrollmentCommand(request.InternId, track.Data.Id), ct);   
            if (!enrollment.Success)
                return RequestResponse<int>.Fail(ErrorCode.InternHasActiveOrPendingEnrollments);

            var payment = await _mediator.Send(new CreateCompletedPaymentCommand(enrollment.Data, track.Data.Fees, request.Method), ct);
            if (!payment.Success)
                return RequestResponse<int>.Fail(ErrorCode.PaymentNotCompleted);

            var accept = await _mediator.Send(new AcceptInternCommand(request.InternId), ct);
            if (!accept.Success)
                return RequestResponse<int>.Fail(ErrorCode.InvalidInternId);

            return RequestResponse<int>.OK(enrollment.Data);
        }
    }
}
