using LMS___Mini_Version.CQRS.Enrollments.Commands.Requests;
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;

namespace LMS___Mini_Version.CQRS.Enrollments.Validators
{
    public class CancelEnrollmentValidator
    {
        private readonly IUnitOfWork _unitOf;
        public CancelEnrollmentValidator(IUnitOfWork unitOf)
        {
            _unitOf = unitOf;
        }

        public async Task<string?> ValidateAsync(CancelEnrollmentCommand command)
        {
            var enrollment = await _unitOf.Enrollments.GetByIdAsync(command.EnrollmentId);
            if (enrollment == null)
                throw new InvalidOperationException($"enrollment with id {command.EnrollmentId} doesn't exist");

            if (enrollment.Status == EnrollmentStatus.Cancelled)
                throw new InvalidOperationException($"enrollment with id {command.EnrollmentId} is already canceled");

            return null;
        }

    }
}
