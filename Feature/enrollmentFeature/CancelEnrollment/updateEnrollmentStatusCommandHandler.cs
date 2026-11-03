using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.CancelEnrollment
{
    public class updateEnrollmentStatusCommandHandler : IRequestHandler<updateEnrollmentStatusCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;

        public updateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepo = enrollmentrepo;
        }

        public async Task<Unit> Handle(updateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
            var enrollmententity = await _enrollmentrepo.GetByIdAsync(request.enrollmentId);

            if (enrollmententity == null)
            {
                throw new KeyNotFoundException($"Enrollment {request.enrollmentId} not found.");
            }

            enrollmententity.Status = request.Status;
            _enrollmentrepo.Update(enrollmententity);
            await _enrollmentrepo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
