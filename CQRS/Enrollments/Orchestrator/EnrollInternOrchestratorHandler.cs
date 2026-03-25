using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.CQRS.Interns.Query;
using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Infrastructure.DTO_S.EnrollmentDTO_s;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orchestrator
{
    public class EnrollInternOrchestratorHandler(IMediator _mediator) : IRequestHandler<EnrollInternOrchestratorRequest, RequestResult<EnrollmentDTO>>
    {

        public async Task<RequestResult<EnrollmentDTO>> Handle(EnrollInternOrchestratorRequest request, CancellationToken cancellationToken)
        {
            //confirm if intern is Exist
            var internResult = await _mediator.Send(new GetInternByIdQuery(request.internId));
            if (internResult == null)
            {
                return RequestResult<EnrollmentDTO>.Failure(ErrorCode.InternNotFound);
            }

            //Check if it has track
            var TrackResult = await _mediator.Send(new GetTrackByIdQuery(internResult.TrackId));
            if (!TrackResult.IsSuccess)
            {
                return RequestResult<EnrollmentDTO>.Failure(ErrorCode.TrackNotFound);
            }
            //check if track is active
            var IsActivetrack = await _mediator.Send(new CheckTrackActivitionQuery(internResult.TrackId));

            //CheckTrackCapacity
            var trackCapacityResult = await _mediator.Send(new CheckCapacityQuery(internResult.TrackId));
            if (!trackCapacityResult.Data)
            {
                return RequestResult<EnrollmentDTO>.Failure(ErrorCode.TrackFull);
            }

            //create new Enrollment
            var Enrollment = await _mediator.Send(new CreateNewEnrollmentCommand(internResult.TrackId, request.internId));
 
            //stage payment
            var payment = await _mediator.Send(new StageNewPaymentCommand(Enrollment.Id, TrackResult.Data.Fees, request.PaymentMethod));

            return RequestResult<EnrollmentDTO>.Success(Enrollment.toEnrollmentDTO());

        }
    }
}
