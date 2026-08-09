using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record UpdateEnrollmentTrackCommand(int EnrollmentId, int NewTrackId) : IRequest;


    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IMediator _mediator;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository, IMediator mediator)
        {
            _enrollmentRepository = enrollmentRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            // the correct way of Update use Bulk Update or use Update by reflection to update the updated fields only
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId));

            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment not found.");


            var enrollmentEntity = new Enrollment { TrackId = request.NewTrackId };
            _enrollmentRepository.Update(enrollmentEntity);

            return Unit.Value;
        }
    }

}
