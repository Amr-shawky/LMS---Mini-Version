using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public UpdateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentId);
            if (enrollment != null)
            {
                enrollment.Status = request.NewStatus;
                _enrollmentRepo.Update(enrollment);
            }
        }
    }
}
