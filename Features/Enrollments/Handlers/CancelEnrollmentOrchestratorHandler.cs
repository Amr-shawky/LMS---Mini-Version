using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers;

public class CancelEnrollmentOrchestratorHandler(IMediator _mediator) : IRequestHandler<CancelEnrollmentOrchestratorCommand, Unit>
{

  

    public async Task<Unit> Handle(CancelEnrollmentOrchestratorCommand request, CancellationToken cancellationToken)
    {
       
        var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
        if (enrollment == null)
            throw new Exception("Enrollment not found");

       
        await _mediator.Send(new UpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);

       
        await _mediator.Send(new RefundPaymentCommand(request.EnrollmentId), cancellationToken);

        return Unit.Value;
    }
}