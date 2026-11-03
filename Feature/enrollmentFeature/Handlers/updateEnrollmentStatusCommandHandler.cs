using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;
namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;
        
        public UpdateEnrollmentStatusCommandHandler(IGeneralRepository<Enrollment> enrollmentrepo) 
        {
            _enrollmentrepo = enrollmentrepo;
        }
        public async Task<Unit> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {

            var enrollmententity = await _enrollmentrepo.GetByIdAsync(request.enrollmentId);

            if (enrollmententity == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {request.enrollmentId} not found.");
            }
            
            enrollmententity.Status = request.Status;

            _enrollmentrepo.SaveInclude(enrollmententity, nameof(Enrollment.Status));

            await _enrollmentrepo.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}