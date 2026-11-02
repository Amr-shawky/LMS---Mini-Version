using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Queries;
using LMS___Mini_Version.Features.Enrollment.UpdateEnrollmentTrack.Command;
using LMS___Mini_Version.Features.Track.GetTrackById.Query;

namespace LMS___Mini_Version.Features.Enrollment.TransferEnrollment.Orchestrator
{
    public record TransferEnrollmentOrchestrator(int enrollmentId, int newTrackId):IRequest<bool>;
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndPoint;
        public TransferEnrollmentOrchestratorHandler(IUnitOfWork unitOfWork,IMediator mediator,IPublishEndpoint publishEndPoint)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _publishEndPoint = publishEndPoint;
        }
        public async Task<bool> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var flag = true;
            await _unitOfWork.ExecuteAsync(async () =>
            {
                var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentId));
                if(enrollment == null)
                {
                    flag = false;
                    return;
                }
                var oldTrackId = enrollment.TrackId;
                var trackId = await _mediator.Send(new GetTrackByIdQuery(request.newTrackId));
                if(trackId == null)
                {
                    flag = false;
                    return;
                }
                if(enrollment.TrackId == request.newTrackId)
                {
                    flag = false;
                    return;
                }


                var updated = await _mediator.Send(new UpdateEnrollmentTrackCommand(request.enrollmentId, request.newTrackId));

                if (!updated)
                {
                    flag = false;
                    return;
                }
                _unitOfWork.AddSavePoint("SavePoint1");
                await _publishEndPoint.Publish(new EnrollmentTransferedMessage(request.enrollmentId, oldTrackId, request.newTrackId, enrollment.InternId));
            });

            return flag;
        }
    }
}
