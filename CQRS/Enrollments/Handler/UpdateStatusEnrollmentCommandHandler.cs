using LMS___Mini_Version.CQRS.Enrollments.Command;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Handler
{
    public class UpdateStatusEnrollmentCommandHandler(IGeneralRepository<Enrollment> _repository) : IRequestHandler<UpdateStatusEnrollmentCommand, bool>
    {
        public async Task<bool> Handle(UpdateStatusEnrollmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var enroll =await _repository.GetById(request.enrollmentId);
                if (enroll == null)
                {
                    throw new NotImplementedException();
                }
                enroll.Status = request.newStatus;

                
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            return await Task.FromResult(true);
        }
    }

}
