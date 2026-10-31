using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    /// <summary>
    /// Handles updateEnrollmentTrackCommand by updating ONLY the TrackId column on the Enrollment row.
    /// This uses SaveInclude so that no other columns (InternId, Status, EnrollmentDate) are touched.
    ///
    /// Why SaveInclude instead of Update(entity)?
    ///   Update() marks every property Modified. Here we only want TrackId changed.
    ///   SaveInclude scopes the SQL UPDATE to just the named column, which is safer and clearer.
    ///
    /// Single repository (R8): only IGeneralRepository of Enrollment is injected.
    /// </summary>
    public class updateEnrollmentTrackCommandHandler : IRequestHandler<updateEnrollmentTrackCommand, RequestResponse<bool>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public updateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<RequestResponse<bool>> Handle(updateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            // Create a stub entity with only the fields needed for a partial update.
            // SaveInclude will attach this, mark TrackId as Modified, and leave the rest unchanged.
            var enrollment = new Enrollment
            {
                Id = request.EnrollmentID,
                TrackId = request.TrackID
            };

            _enrollmentRepository.SaveInclude(enrollment, nameof(Enrollment.TrackId));
            await _enrollmentRepository.SaveChangesAsync();

            return RequestResponse<bool>.Ok(true, "Enrollment track updated successfully.");
        }
    }
}