using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class UpdateEnrollmentTrackCommandHandler : IRequestHandler<UpdateEnrollmentTrackCommand, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;

        public UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentrepo) =>
            _enrollmentrepo = enrollmentrepo;

        public async Task<Unit> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {

            var enrollment = await _enrollmentrepo.GetByIdAsync(request.EnrollmentId);

            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment with ID {request.EnrollmentId} not found.");
            
            enrollment.TrackId = request.TrackId;

            _enrollmentrepo.SaveInclude(enrollment, nameof(Enrollment.TrackId));
            await _enrollmentrepo.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
