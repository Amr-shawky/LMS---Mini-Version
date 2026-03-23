using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class UpdateEnrollmentTrackCommandHandler(IGeneralRepository<Enrollment> _repository) : IRequestHandler<UpdateEnrollmentTrackCommand, bool>
    {
        public async Task<bool> Handle(UpdateEnrollmentTrackCommand request, CancellationToken cancellationToken)
        {
            var enroll =await _repository.GetById(request.enrollmentId);
            if (enroll == null)
            {
                throw new NotImplementedException();
            }
            enroll.TrackId = request.newTrackId;
            _repository.Update(enroll);
            return await Task.FromResult(true);

        }
    }
}
