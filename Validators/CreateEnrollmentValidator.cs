using FluentValidation;
using LMS___Mini_Version.ViewModels.Enrollment;

namespace LMS___Mini_Version.Validators
{
    public class CreateEnrollmentValidator : AbstractValidator<EnrollInternViewModel>
    {
        public CreateEnrollmentValidator()
        {
            RuleFor(x => x.InternId)
                .GreaterThan(0).WithMessage("Intern ID must be a positive number.");

            RuleFor(x => x.TrackId)
                .GreaterThan(0).WithMessage("Track ID must be a positive number.");
        }
    }
}
