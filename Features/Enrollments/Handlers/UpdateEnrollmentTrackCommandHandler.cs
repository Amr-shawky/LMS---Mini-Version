using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Enrollments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentId);
            if (enrollment != null)
            {
                enrollment.TrackId = request.NewTrackId;
                _enrollmentRepo.Update(enrollment);
            }
        }
    }
}
