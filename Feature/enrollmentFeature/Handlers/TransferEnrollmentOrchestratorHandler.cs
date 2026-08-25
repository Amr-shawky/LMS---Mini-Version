using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, Unit>
    {
        private readonly IMediator _mediator;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Unit> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // step 1 enrollment validation 
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID));

            //valid enrollment exist 
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"enrollemnt {request.EnrollmentID} not found");
            }

            // check if cancelled 

            if(enrollment.Status == Domain.Enums.EnrollmentStatus.Cancelled)
            {
                throw new InvalidOperationException($"cannot transfer a cancelled enrollment ");
            }

            // check if the track alredy enrollment 

            if(enrollment.TrackId == request.newTrackID)
            {
                throw new InvalidOperationException("the intern already enrolled this track");
            }


            //-----------------------

            //step 2 : track validation 

            var track = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID));

            // track is exist 

            if (track == null)
            {
                throw new KeyNotFoundException($"track {request.newTrackID} not found");

            }

            // is the track active  

            if (!track.IsActive)
            {
                throw new InvalidOperationException($"track {request.newTrackID} not active");
            
            }


            //step 3 validate active capicity 

            var activeenrolledcount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.newTrackID));
            
            // reached max capicity ? 
            if(activeenrolledcount >= track.MaxCapacity)
            {
                throw new InvalidOperationException("target track has reached maximum capacity ");
            }

            //-------------------------

            //step 4 update track ID 

            await _mediator.Send(new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID));

            if (track.Fees > 0)
            {

                await _mediator.Send(new refundPaymentCommand(request.EnrollmentID));
            }

            return Unit.Value;
        }
    }
}
