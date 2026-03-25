using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.CQRS.Enrollments.Query;
using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Enums;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<RequestResult<bool>> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var Enrollment =await _mediator.Send(new GetByIdEnrollmentQuery(request.oldTrackId));
            if (!Enrollment.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.EnrollMentNotExist);

            var track =await _mediator.Send(new GetTrackByIdQuery(request.oldTrackId));
            if (!track.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.TrackNotFound);

            var CheckCapacity =await _mediator.Send(new CheckCapacityQuery(request.oldTrackId));
            if (!CheckCapacity.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.TrackFull);

            var UpdatedTrack= await _mediator.Send(new UpdateEnrollmentTrackCommand(request.oldTrackId,request.newTrackId));
            if (!UpdatedTrack)
                return RequestResult<bool>.Failure(ErrorCode.UpdateError);


            var NewTrack = await _mediator.Send(new GetTrackByIdQuery(request.newTrackId));
            if (!NewTrack.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.TrackNotFound);


            var paymentResult =await _mediator.Send(new UpdatePaymentAmountCommand(Enrollment.Data.PaymentId.Value,NewTrack.Data.Fees));

            if (!paymentResult.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.paymentNotEdited);
            return RequestResult<bool>.Success(true);


        }
    }
}
s