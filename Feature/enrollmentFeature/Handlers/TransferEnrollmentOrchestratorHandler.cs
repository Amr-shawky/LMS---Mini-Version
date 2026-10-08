using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            // step 1 enrollment validation 
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID), cancellationToken);

            // valid enrollment exist 
            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
            {
                return RequestResponse.Fail($"Enrollment {request.EnrollmentID} not found", 404);
            }

            var enrollment = enrollmentResponse.Data;

            // check if cancelled 
            if (enrollment.Status == Domain.Enums.EnrollmentStatus.Cancelled)
            {
                return RequestResponse.Fail("Cannot transfer a cancelled enrollment", 400);
            }

            // check if already enrolled in this track 
            if (enrollment.TrackId == request.newTrackID)
            {
                return RequestResponse.Fail("The intern is already enrolled in this track", 400);
            }

            //-----------------------

            // step 2 : track validation 
            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), cancellationToken);

            // track exists 
            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse.Fail($"Track {request.newTrackID} not found", 404);
            }

            var track = trackResponse.Data;

            // is the track active  
            if (!track.IsActive)
            {
                return RequestResponse.Fail($"Track {request.newTrackID} is not active", 400);
            }

            // step 3 validate active capacity 
            var activeenrolledcount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.newTrackID), cancellationToken);

            // reached max capacity ? 
            if (activeenrolledcount >= track.MaxCapacity)
            {
                return RequestResponse.Fail("Target track has reached maximum capacity", 400);
            }

            //-------------------------

            // step 4 update track ID 
            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _mediator.Send(new updateEnrollmentTrackCommand(request.EnrollmentID, request.newTrackID), cancellationToken);

                if (track.Fees > 0)
                {
                    await _unitOfWork.AddSavePointAsync("RefundSavePoint");
                    try
                    {
                        await _mediator.Send(new refundPaymentCommand(request.EnrollmentID), cancellationToken);
                    }
                    catch
                    {
                        await _unitOfWork.RollbackToSavePointAsync("RefundSavePoint");
                    }
                }
            });

            return RequestResponse.Ok("Enrollment transferred successfully");
        }
    }
}
