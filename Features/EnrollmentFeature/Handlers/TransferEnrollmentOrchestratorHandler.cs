using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.EnrollmentFeature.Commands;
using LMS___Mini_Version.Features.EnrollmentFeature.Orchestrators;
using LMS___Mini_Version.Features.EnrollmentFeature.Quries;
using LMS___Mini_Version.Features.Shared;
using LMS___Mini_Version.Features.TrackFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.EnrollmentFeature.Handlers
{
    public class TransferEnrollmentOrchestratorHandler : IRequestHandler<TransferEnrollmentOrchestrator, RequestResponse<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<bool>> Handle(TransferEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.enrollmentID));

            
            if (enrollment.Data.TrackId == request.newTrackID)
            {
                //fail
                throw new InvalidOperationException("you cannot transfer to the same track");
            }

            if (enrollment.Data.Status==Domain.Enums.EnrollmentStatus.Cancelled)
            {
                throw new InvalidOperationException("enrollment is cancelled");

            }

            var track = await _mediator.Send(new GetTrackByIDQuery(request.newTrackID));

            if(track == null)
            {
                // fail
            }

            var enrollmentcount = await _mediator.Send(new GetActiveEnrollmentCountByTrackQuery(request.newTrackID));

            if( enrollmentcount.Data >= track.Data.MaxCapacity)
            {
                // error
            }

            // updateenrollmenttrackCommand
            var result = await _mediator.Send(new UpdateEnrollmentTrackCommand(request.enrollmentID, request.newTrackID));



            // update payment Amount command 
            await _unitOfWork.SaveChangesAsync();

            return RequestResponse<bool>.OK(200,true);
        }
    }
}
