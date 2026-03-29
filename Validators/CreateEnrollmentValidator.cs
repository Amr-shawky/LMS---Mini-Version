using FluentValidation;
using LMS___Mini_Version.CQRS.Enrollments.Commands;

namespace LMS___Mini_Version.Validators
{
    public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentCommand>
    {
        public CreateEnrollmentValidator()
        {
            RuleFor(x => x.internId)
                .GreaterThan(0).WithMessage("Intern ID must be a positive number.");

            RuleFor(x => x.trackId)
                .GreaterThan(0).WithMessage("Track ID must be a positive number.");
        }
    }
}