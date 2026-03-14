using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class EnrollInternOrchestratorHandler : IRequestHandler<EnrollInternOrchestratorRequest, EnrollmentResultDto>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        
        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<EnrollmentResultDto> Handle(EnrollInternOrchestratorRequest request, CancellationToken cancellationToken)
        {
            var intern = await _mediator.Send(new ValidateInternExistsQuery(request.InternId), cancellationToken);
            if (!intern)
                return EnrollmentResultDto.Fail("Intern does not exist.");
            
            var track = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (track == null) 
                return EnrollmentResultDto.Fail("Track does not exist.");

            if (!track.IsActive)
                return EnrollmentResultDto.Fail("Track is not active.");

            var hasCapacity = await _mediator.Send(new CheckTrackCapacityQuery(request.TrackId), cancellationToken);
            if (!hasCapacity)
                return EnrollmentResultDto.Fail("Track has reached its capacity.");

            var enrollment = await _mediator.Send(new StageEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

            await _unitOfWork.CompleteAsync();

            PaymentDto? payment = null; 
            if (track.Fees > 0)
            {
                var paymentEntity = await _mediator.Send(new StagePaymentCommand(enrollment.Id, track.Fees, PaymentMethod.Cash));
                await _unitOfWork.CompleteAsync();

                payment = paymentEntity.ToDto();
            }

            return EnrollmentResultDto.Succeed(enrollment.ToDto(), payment);
        }
    }
}
