using LMS___Mini_Version.CQRS.Enrollments.Commands;
using LMS___Mini_Version.CQRS.Intern.Queries;
using LMS___Mini_Version.CQRS.Payments.Commands;
using LMS___Mini_Version.CQRS.Tracks.Queries;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Orcherstrators;

public class CreateEnrollmentOrcherstratorHandler( IMediator _mediator,IUnitOfWork _unitOfWork)
    : IRequestHandler<CreateEnrollmentOrcherstratorRequest, RequestResult<EnrollmentDto>>
{

    
    public async Task<RequestResult<EnrollmentDto>> Handle(CreateEnrollmentOrcherstratorRequest request, CancellationToken cancellationToken)
    {
        var intern = await _mediator.Send(new GetInternByIdQuery(request.InternId), cancellationToken);
        if (intern == null) 
        {
            return RequestResult<EnrollmentDto>.Failure(ErrorCode.InternNotFound, $"Intern with ID {request.InternId} was not found.");
        }
        // track exist 

        var track = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
        if (track == null)
        {
            return RequestResult<EnrollmentDto>.Failure(ErrorCode.TrackNotFound, $"Track with ID {request.TrackId} was not found.");
        }
        if (!track.IsActive) 
        {
            return RequestResult<EnrollmentDto>.Failure(ErrorCode.TrackInactive, $"Track '{track.Name}' is not currently active.");
        }

        var hasCapacity = await _mediator.Send(new CheckCapacityTrackQuery(request.TrackId), cancellationToken);

        if (!hasCapacity) 
        {
            return RequestResult<EnrollmentDto>.Failure(ErrorCode.TrackAtMaxCapacity, $"Track '{track.Name}' has reached its maximum capacity.");
        }

        var enrollement = await _mediator.Send(new CreateEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

        // enrollement should added cuz id still 0 

        PaymentDto? payment = null;
        if (track.Fees > 0) 
        {
            payment = await _mediator.Send(new CreatePaymentCommand(enrollement.Id, track.Fees, PaymentMethod.Cash), cancellationToken);
        }

        // call save changes here or in controller 
        await _unitOfWork.CompleteAsync();

        return RequestResult<EnrollmentDto>.Sucess(enrollement);

    }
}
