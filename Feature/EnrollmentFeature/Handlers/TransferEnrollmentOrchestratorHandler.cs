using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Orchestrators;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Feature.Tracks.Query;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.Handlers
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
            // Step 1: Get the existing enrollment : 
            var enrollmentResponse = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentID), cancellationToken);
            
            if (!enrollmentResponse.Success || enrollmentResponse.Data == null)
            {
                return RequestResponse.Fail($"Enrollment with ID {request.EnrollmentID} was not found.",404);
            }

            var enrollment = enrollmentResponse.Data;

            // Step 2: Get the existing track :
            var trackResponse = await _mediator.Send(new GetByIdTrackQuery(request.newTrackID), cancellationToken);

            if (!trackResponse.Success || trackResponse.Data == null)
            {
                return RequestResponse.Fail($"Track with ID {request.newTrackID} was not found.", 404);
            }

            var newTrack = trackResponse.Data;

            // Step 3: Validate the destination track
            if (!newTrack.IsActive)
            {
                return RequestResponse.Fail("Cannot transfer enrollment to an inactive track.",400);
            }

            //4. Check the active enrollment count for the new track :
            var activeEnrollmentCount = await _mediator.Send(new getActiveEnrollmentCountByTrackQuery(request.newTrackID), cancellationToken);

            //5. Check if the new track has reached its maximum capacity :
            if (activeEnrollmentCount >= newTrack.MaxCapacity)
            {
                return RequestResponse.Fail("The destination track has reached its maximum capacity.", 400); 
            }
            
            //6.check if the enrollment is already in the new track :
            if(enrollment.TrackId == request.newTrackID)
            {
                return RequestResponse.Fail("The enrollment is already in the specified track.", 400);
            }

            //7. Update the enrollment's track ID to the new track ID :

            RequestResponse result = RequestResponse.Fail("Failed to transfer enrollment.", 500);

            try
            {
                await _unitOfWork.ExecuteAsync(async () =>
                {
                    var updateResult = await _mediator.Send(new UpdateEnrollmentTrackCommand(enrollment.Id, request.newTrackID), cancellationToken);

                    if(!updateResult.Success)
                        throw new InvalidOperationException("Failed to update enrollment track.");

                    result = RequestResponse.Ok("Enrollment transferred successfully.", 200);
                });

            }
            catch(InvalidOperationException ex)
            {
                result = RequestResponse.Fail(ex.Message, 500);
            }

            return result;
        }
    }
}
