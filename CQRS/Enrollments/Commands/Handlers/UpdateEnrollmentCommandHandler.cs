using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Exceptions;
using MediatR;

namespace LMS___Mini_Version.CQRS.Enrollments.Commands.Handlers
{
    public class UpdateEnrollmentCommandHandler : IRequestHandler<UpdateEnrollmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEnrollmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.id);

            if (enrollment == null)
                throw new NotFoundException(nameof(Domain.Entities.Enrollment), request.id);

            if (!Enum.TryParse<EnrollmentStatus>(request.status, true, out var status))
                throw new BadRequestException(
                    $"Invalid status: '{request.status}'. Allowed: Pending, Active, Completed, Cancelled");

            enrollment.Status = status;
            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
