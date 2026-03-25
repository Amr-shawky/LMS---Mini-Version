using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class CreateNewEnrollmentCommandHandler(IGeneralRepository<Enrollment> _repository) : IRequestHandler<CreateNewEnrollmentCommand, Enrollment>
    {
        
        public Task<Enrollment> Handle(CreateNewEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var Enroll=new Enrollment()
            {
                InternId=request.internId,
                TrackId=request.trackId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending,
                
            };
            _repository.Add(Enroll);
            return Task.FromResult(Enroll);
            
        }
    }
}
