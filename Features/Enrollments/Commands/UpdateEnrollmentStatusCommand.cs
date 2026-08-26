using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Commands
{
    public record UpdateEnrollmentStatusCommand(int EnrollmentId, EnrollmentStatus NewStatus) : IRequest;


        public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand, Unit>
        {
            private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
            private readonly IMediator _mediator;

            public UpdateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository , IMediator mediator)
            {
                _enrollmentRepository = enrollmentRepository;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
            {
               // the correct way of Update use Bulk Update or use Update by reflection to update the updated fields only
               //var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId));
              var enrollment = await _enrollmentRepository
                                    .GetAll()
                                    .Include(e => e.Intern)
                                    .Include(e => e.Track)
                                    .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);
            if (enrollment == null)
                    throw new KeyNotFoundException($"Enrollment not found.");

                enrollment.Status = request.NewStatus;
                _enrollmentRepository.Update(enrollment);

                return Unit.Value;
            }
        }
    }

