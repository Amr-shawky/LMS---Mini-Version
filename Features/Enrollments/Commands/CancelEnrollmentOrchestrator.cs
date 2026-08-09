using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record CancelEnrollmentOrchestrator(int EnrollmentId) : IRequest;

    public class CancelEnrollmentOrchestratorHandler : IRequestHandler<CancelEnrollmentOrchestrator, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CancelEnrollmentOrchestratorHandler(
            IGeneralRepository<Enrollment> enrollmentRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        // 1- check enrollment was found and it cancelled or not
        //2- if not cancelled , cancel it and update payment status to refund 

        public async Task<Unit> Handle(CancelEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.Cancelled)
                throw new InvalidOperationException("This enrollment is already cancelled.");

            await _mediator.Send(new UpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);

            await _mediator.Send(new RefundPaymentCommand(request.EnrollmentId), cancellationToken);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}