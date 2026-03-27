using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Infrastructure.Repositories;
using LMS___Mini_Version.Persistence;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollment.Commands.HandlerCommands
{
    public class UpdateEnrollmentCommandHandler:IRequestHandler<UpdateEnrollmentCommand, bool>
    {
        IUnitOfWork _unitOfWork;
        public  Task<bool> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Enrollments.Update(new Domain.Entities.Enrollment
            {
                Id = request.id,
                InternId = request.internId,
                TrackId = request.trackId
            });
            return Task.FromResult(true);
        }
    }
}
