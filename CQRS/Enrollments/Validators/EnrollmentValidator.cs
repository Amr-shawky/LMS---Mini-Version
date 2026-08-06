using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;
using LMS___Mini_Version.Domain.Repositories;

namespace LMS___Mini_Version.CQRS.Enrollments.Validators
{
    public class EnrollmentValidator
    {

        private readonly IUnitOfWork _unitOf;
        public EnrollmentValidator(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<string?> ValidateAsync(EnrollmentInternCommand command)
        {
            var intern = await _unitOf.Interns.GetByIdAsync(command.InternId);
            if (intern == null)
                return $"Intern with Id {command.InternId} doesn't exist";

            var track = _unitOf.Tracks.GetByIdAsync(command.TrackId);
            if (track == null)
                return "Track doesn't exist";

            var hasCapacity = await _unitOf.Tracks.CheckCapacityAsync(command.TrackId);
            if (!hasCapacity)
                return "Track is at full capacity";

            var isDuplicate = await _unitOf.Enrollments.HasActiveEnrollmentAsync(command.InternId, command.TrackId);
            if (isDuplicate)
                return $"This intern with Id {command.InternId} already has an active enrollment";

            var isActive = await _unitOf.Tracks.IsTrackActiveAsync(command.TrackId);
            if (!isActive)
                return "This track is not active";

            return null;
        }

    }
}
