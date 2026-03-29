using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Exceptions;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Handlers
{
    public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEnrollmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.id);

            if (enrollment == null)
                throw new NotFoundException(nameof(Domain.Entities.Enrollment), request.id);

            _unitOfWork.Enrollments.Delete(enrollment);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
