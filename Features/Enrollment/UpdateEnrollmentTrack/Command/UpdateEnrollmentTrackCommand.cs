

namespace LMS___Mini_Version.Features.Enrollment.UpdateEnrollmentTrack.Command
{
    public record UpdateEnrollmentTrackCommand(int enrollmentId, int newTrackId)
    : IRequest<bool>;

    public class UpdateEnrollmentTrackCommandHandler
        : IRequestHandler<UpdateEnrollmentTrackCommand, bool>
    {
        private readonly IGenericRepository<Domain.Entities.Enrollment, int> _repository;

        public UpdateEnrollmentTrackCommandHandler(
            IGenericRepository<Domain.Entities.Enrollment, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateEnrollmentTrackCommand request,
            CancellationToken cancellationToken)
        {
            var enrollment = await _repository.GetByIdAsync(request.enrollmentId);

            if (enrollment == null)
                return false;

            enrollment.TrackId = request.newTrackId;

            _repository.SaveInclude(
                enrollment,
                nameof(LMS___Mini_Version.Domain.Entities.Enrollment.TrackId));

            return true;
        }
    }
}
