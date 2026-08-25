using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Feature.enrollmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Feature.enrollmentFeature.Handlers
{
    public class updateEnrollmentTrackCommandHandler : IRequestHandler<updateEnrollmentTrackCommand, Unit>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentrepo;
        private readonly IUnitOfWork _unitOfWork;
        public updateEnrollmentTrackCommandHandler(IUnitOfWork unitOfWork,
            IGeneralRepository<Enrollment> enrollmentrepo)
        {
            _enrollmentrepo = enrollmentrepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(updateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentrepo.GetAll().
                FirstOrDefaultAsync(e => e.Id == request.EnrollmentID);
            if(enrollment == null)
            {
                throw new KeyNotFoundException($"enrollment {request.EnrollmentID} not found");
            }

            enrollment.TrackId = request.TrackID;

             _enrollmentrepo.Update(enrollment);

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
