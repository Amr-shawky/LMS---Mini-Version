using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.TransferEnrollment
{
    public class updateEnrollmentTrackCommandHandler : IRequestHandler<updateEnrollmentTrackCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public updateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Unit> Handle(updateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            // Use SaveInclude to selectively update TrackId without full entity fetch
            var enrollment = new Enrollment
            {
                Id = request.EnrollmentID,
                TrackId = request.TrackID
            };

            _enrollmentRepository.SaveInclude(enrollment, nameof(Enrollment.TrackId));
            await _enrollmentRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

