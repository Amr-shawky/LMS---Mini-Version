using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class updateEnrollmentTrackCommandHandler : IRequestHandler<updateEnrollmentTrackCommand>
    {
        private readonly IGeneralRepository<Enrollment> _repo;
      

        public updateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(updateEnrollmentTrackCommand request, CancellationToken ct)
        {
            var enrollment = await _repo.GetByIdAsync(request.EnrollmentID);
            if (enrollment is null)
                throw new KeyNotFoundException($"Enrollment {request.EnrollmentID} not found");

            enrollment.TrackId = request.TrackID;

            _repo.SaveInclude(enrollment, nameof(Enrollment.TrackId));

            await _repo.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
