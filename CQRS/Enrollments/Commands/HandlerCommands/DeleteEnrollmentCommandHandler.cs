using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.HandlerCommands
{
    public class DeleteEnrollmentCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteEnrollmentCommand, bool>
    {
        public async Task<bool> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.id);

            if (enrollment == null)
                return false;

            _unitOfWork.Enrollments.Delete(enrollment);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
